import type { Dish } from '../lib/services/DishService';
import type { Menu }  from '../lib/services/MenuService' 



export default interface Ticket {
    id: number;
    dishes: Dish;    
    quantity: number;
}

export interface Order {
    ticket: Ticket[];
    ticketNumber: number;
    menuId: string;
}

export interface MemoryCache {
    menus: Menu[];
    currentMenu: Menu | null;
    orders: Order[]; 
    currentOrder: Order | null;
    isLoading: boolean;
    lastFetch: number | null;
    cacheExpiry: number;
    isStale: boolean | null;
}

const CacheDuration = 1000 * 60 * 15; // 15 minutes
const CACHE_KEY = 'menuCache'; // This is the Cache name, 


const activeCache: MemoryCache = {
    menus: [],
    currentMenu: null,
    orders: [],
    currentOrder: null,
    isLoading: false,
    lastFetch: null,
    cacheExpiry: CacheDuration,
    isStale: null
};

// When updating the cache, always update database. First database, then cache.


function saveToCache(cache: MemoryCache) {
    if (typeof window === 'undefined') return; // Skip on server
    
    try{
        const cacheData = {
            menus: cache.menus,
            currentMenu: cache.currentMenu,
            orders: cache.orders,
            timestamp: Date.now()
        };
        localStorage.setItem(CACHE_KEY, JSON.stringify(cacheData));
    } catch (error) {
/*      might need to implement a clearing here if data exceeded limit*/
        console.error('Error saving to cache:', error);
    }     
}

function loadFromCache(): Partial<MemoryCache> | null {
    if (typeof window === 'undefined') return null; // Skip on server
    
    try {
        const cached = localStorage.getItem(CACHE_KEY);
        if (!cached) return null;
        
        const cachedData = JSON.parse(cached);
        const age = Date.now() - cachedData.timestamp;
        const isStale = age > CacheDuration;

        return {
            menus: cachedData.menus || [],
            currentMenu: cachedData.currentMenu || null,
            orders: cachedData.orders || null,
            isStale,
            lastFetch: cachedData.lastFetch
        };
    }
    catch (error) {
        console.error('Error loading from cache:', error);
        return null;
    }
}

function clearCache() {
    if (typeof window === 'undefined') return; // Skip on server
    
    try {
        localStorage.removeItem(CACHE_KEY);
    } catch (error) {
        console.error('Error clearing cache:', error);
    }
}

export const cacheHandlerActions = {
    initializeCache: () => {
        const cached = loadFromCache();
        if (cached) {
            activeCache.menus = cached.menus || [];
            activeCache.currentMenu = cached.currentMenu || null;
            activeCache.orders = cached.orders || [];
            activeCache.isStale = cached.isStale ?? null;
            activeCache.lastFetch = cached.lastFetch ?? null;
        }
        },

        setLoading(loading: boolean) {
            activeCache.isLoading = loading;
        },

        setMenus(menus: Menu[]) {
            activeCache.menus = menus;
            activeCache.lastFetch = Date.now();
            activeCache.isStale = false;
            saveToCache(activeCache);
        },

        setCurrentMenu(menu: Menu | null) {
            activeCache.currentMenu = menu;
            saveToCache(activeCache);
        },

        addMenu(menu: Menu) {
            activeCache.menus.push(menu);
            saveToCache(activeCache);
        },

        addDish(menuId: string, dish: Dish) {
            const menu = activeCache.menus.find(m => m.menuId === menuId);
            if(menu) {
                menu.dishes.push(dish);
                saveToCache(activeCache);            
            }
            else {
                console.warn(`Could not find menu in cache with id: ${menuId}`);
            }

        },

        updateMenu(updatedMenu: Menu) {
            const index = activeCache.menus.findIndex(m => m.menuId === updatedMenu.menuId);
            if(index !== -1){
                activeCache.menus[index] = updatedMenu;
                saveToCache(activeCache);
            }   
        },


        updateDish(menuId: string, updatedDish: Dish) {
            const menu = activeCache.menus.find(m => m.menuId === menuId);
            if(menu){
                const index = menu.dishes.findIndex(d => d.Id === updatedDish.Id);
                if(index !== -1) // findIndex returns -1 if not found
                    {
                    menu.dishes[index] = updatedDish;
                    saveToCache(activeCache);
                }
            }                
        },

        removeDish(menuId: string, dishId: number) {
            const menu = activeCache.menus.find(f => f.menuId === menuId);
            if(!menu) {
                console.warn(`Could not find menu in cache with id: ${menuId}`);
                return;
            }

            const initialLength = menu.dishes.length;
            menu.dishes = menu.dishes.filter(p => p.Id !== dishId);

            if(menu.dishes.length === initialLength) {
                console.warn(`Dish with id ${dishId} not found in menu ${menuId}`);
            }
            saveToCache(activeCache);
        },
        removeMenu(menuId: string) {
            const menuExist = activeCache.menus.some(s => s.menuId === menuId);
            if(!menuExist) {
                console.warn(`Could not find menu in cache with id: ${menuId}`)
                return;
            }

            activeCache.menus = activeCache.menus.filter(f => f.menuId !== menuId);
            saveToCache(activeCache);
        },

        isCacheStale: (): boolean => {
            return activeCache.isStale ?? true;
        },

        invalidateCache: () => {
            clearCache();
            activeCache.menus = [];
            activeCache.currentMenu = null;
            activeCache.orders = [];
            activeCache.lastFetch = null;
            activeCache.isStale = null;
        },
/*         removeCurrentOrder: (menuId: string) => {
            const index = activeCache.orders.findIndex(o => o.menuId === menuId);
            if (index !== -1) {
                activeCache.orders[index].ticket = [];
                saveToCache(activeCache.menus);
                activeCache.currentOrder = null;
            }
        }, */

        //TODO •  addOrder(order: Order)    updateOrder(menuId: string, updatedOrder: Order)    removeOrder(menuId: string)

        //
        //if order exists updates it silently.
        addOrder(order: Order){

            const res = activeCache.orders.findIndex((x, y) => x.menuId == order.menuId)
            if(res !== -1)
            {
                return this.updateOrder(order.menuId, order)
            }

            activeCache.orders.push(order)

            activeCache.isLoading = true;
            saveToCache(activeCache);
            activeCache.isLoading = false;
        },

        removeOrder(menuId: string){
            const res = activeCache.orders.findIndex((x, y) => x.menuId === menuId)
            if(res === -1)
                console.warn(`Could not locate ${menuId} in activeCache`)
            
            //this might be a memeory leak...?
            activeCache.orders = activeCache.orders.filter((x,y)=> x.menuId !== menuId)
            saveToCache(activeCache);
        },

        updateOrder(menuId :string, updateOrder: Order){
            const res = activeCache.orders.findIndex((x,y) => x.menuId === menuId)
            if(res -1){
                console.warn(`could not locate ${menuId} in activeCache`)
                return;
            }

            const ref = activeCache.orders[res];
            ref.ticket = updateOrder.ticket;
            ref.ticketNumber = updateOrder.ticketNumber;
            saveToCache(activeCache);
        },


        getActiveCache: () => activeCache // call this to get the current cache state
};
