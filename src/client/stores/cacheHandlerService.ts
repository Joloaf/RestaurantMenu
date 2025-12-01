import type {  Dish } from '../src/lib/services/DishService';
import type { Menu } from '../src/lib/services/MenuService';



export interface Ticket {
    id: number;
    dishes: Dish;    
    quantity: number;
}

export interface Order {
    ticket: Ticket[];
    ticketNumber: number;
    menuId: number;
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
    currentOrder: [] as unknown as Order | null,
    isLoading: false,
    lastFetch: null,
    cacheExpiry: CacheDuration,
    isStale: null
};

// When updating the cache, always update database. First database, then cache.


function saveToCache(menus: Menu[]) {
    try{
        const cacheData = {
            menus,
            timestamp: Date.now()
        };
        localStorage.setItem(CACHE_KEY, JSON.stringify(cacheData));
    } catch (error) {
        console.error('Error saving to cache:', error);
    }     
}

function loadFromCache(): { menus: Menu[]; isStale: boolean } | null {
    try {
        const cached = localStorage.getItem(CACHE_KEY);
        if (!cached) return null;
        
        const cachedData = JSON.parse(cached);
        const age = Date.now() - cachedData.timestamp;
        const isStale = age > CacheDuration;

        return { menus: cachedData.menus, isStale };
    }
    catch (error) {
        console.error('Error loading from cache:', error);
        return null;
    }
}

function clearCache() {
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
            activeCache.menus = cached.menus;
            activeCache.isStale = cached.isStale;
            activeCache.lastFetch = Date.now() - (cached.isStale ? CacheDuration + 1 : 0);
            }
        },

        setLoading(loading: boolean) {
            activeCache.isLoading = loading;
        },

        setMenus(menus: Menu[]) {
            activeCache.menus = menus;
            activeCache.lastFetch = Date.now();
            activeCache.isStale = false;
            saveToCache(menus);
        },

        setCurrentMenu(menu: Menu | null) {
            activeCache.currentMenu = menu;
        },

        addMenu(menu: Menu) {
            activeCache.menus.push(menu);
            this.setCurrentMenu(menu);
            saveToCache(activeCache.menus);
        },

        addDish(menuId: number, dish: Dish) {
            const menu = activeCache.menus.find(m => m.menuId === menuId);
            if (menu) {
                menu.dishes.push(dish);
                saveToCache(activeCache.menus);
            }
        },

        updateMenu(updatedMenu: Menu) {
            const index = activeCache.menus.findIndex(m => m.menuId === updatedMenu.menuId);
            if(index !== -1){
                activeCache.menus[index] = updatedMenu;
                saveToCache(activeCache.menus);
            }   
        },


        updateDish(menuId: number, updatedDish: Dish) {
            const menu = activeCache.menus.find(m => m.menuId === menuId);
            if(menu){
                const index = menu.dishes.findIndex(d => d.id === updatedDish.id);
                if(index !== -1) // findIndex returns -1 if not found
                    {
                    menu.dishes[index] = updatedDish;
                    saveToCache(activeCache.menus);
                }
            }                
        },

        removeDish(menuId: number, dishId: number) {
            const menu = activeCache.menus.find(m => m.menuId === menuId);
            if(menu) {
                menu.dishes = menu.dishes.filter(d => d.id !== dishId);
                saveToCache(activeCache.menus);
            }
        },
        removeMenu(menuId: number) {
            activeCache.menus = activeCache.menus.filter(m => m.menuId !== menuId);
            saveToCache(activeCache.menus);
        },

        isCacheStale: (): boolean => {
            return activeCache.isStale ?? true;
        },

        invalidateCache: () => {
            clearCache();
            activeCache.menus = [];
            activeCache.currentMenu = null;
            activeCache.orders = [];
            activeCache.currentOrder = null;
            activeCache.lastFetch = null;
            activeCache.isStale = null;
        },
        removeCurrentOrder: (menuId: number) => {
            const index = activeCache.orders.findIndex(o => o.menuId === menuId);
            if (index !== -1) {
                activeCache.orders[index].ticket = [];
                saveToCache(activeCache.menus);
                activeCache.currentOrder = null;
            }
        },
        
        getActiveCache: () => activeCache // call this to get the current cache state
};
