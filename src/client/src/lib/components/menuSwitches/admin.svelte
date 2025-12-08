<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import { ApiService } from "$lib/services/apiService";
    import { MenuService } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";
	import { stopImmediatePropagation } from "svelte/legacy";
	import RestDish from "../RestDish.svelte";
	import { DishService, type Dish } from "$lib/services/DishService";

    const apiService = new ApiService();
    const menuService = new MenuService(apiService);
    const dishService = new DishService(apiService);

    let { menus = $bindable(), currentMenu, assignHandler } = $props<{ menus: Menu[], currentMenu: Menu | null, assignHandler: (handleSwap: () => Promise<void>) => void}>();

    export async function DBUpdatePageData(){
        updateMenus();
        for(let i = 0; i < AdminState.length; i++)
            updateDishes(AdminState[i])
    }
    
    export async function updateMenus(){
        let currMen :Menu[] = cacheHandlerActions.getActiveCache().menus
        for(let i = 0; i < currMen.length; i ++)
        //dropping into javascript cus lazy, should be a helper to compare the fields
            if(!Object.keys(currMen[i]).map((x) => { 
                if(x !== 'dishes') 
                    return AdminState[i][x] === currMen[i][x] //work this will not when smarter the compiler gets
                else return true;
             }).every(x => x))
            {
                cacheHandlerActions.updateMenu(AdminState[i])
            }
    }
    export async function updateDishes(menu: Menu){
        //the aforementioned local helper
        const evalElem = (cacheDish :Dish, adminDish : Dish) :boolean =>{
            return (cacheDish.dishName === adminDish.dishName 
                && cacheDish.dishPicture === adminDish.dishPicture)
        };
        //reference for the cache object
        const cacheDishes = cacheHandlerActions.getActiveCache().menus.find((x) => x.menuId == menu.menuId)

        if(!cacheDishes)
            throw Error(`couldnt locate: ${menu.menuId} in cache, state is disjointed`)

        //loop over, compare, update
        for(let i = 0; i < menu.dishes.length; i++)
            if(!evalElem(cacheDishes.dishes[i], menu.dishes[i])){
                cacheHandlerActions.updateDish(menu.menuId!, menu.dishes[i])
            }
    }
    let currentActiveMenu :string = $state("-1")
    //let AdminState :Menu[] = $state(menus);
    //let currentActiveMenu = $state("-1")
    let AdminState: Menu[] = $state(menus);
    AdminState = cacheHandlerActions.getActiveCache().menus;  //ask marcus

   async function createNewMenu(event : MouseEvent){

        event.stopImmediatePropagation()
        const newMenu: Menu = {
            menuId: "0" , // Temporary ID // why is this a string? why not number?
            menuName: "Defaultmenu",
            userName: "Defautuser",
            theme: "d1bbc886-0a27-4f95-9bf1-7ed9758694c7.webp", // Default theme image
            dishes: []
        };
    
       const menu = await menuService.createMenu(newMenu);
       if(menu.menuId != null){
           menu.menuId = menu.menuId.toString();
   
          cacheHandlerActions.addMenu(menu);
          AdminState.push(menu)
       }
    }

    function onClickDelete(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
    
	}

    async function addDish() {
        const newDish: Dish = {
            id: 0, // Temporary ID
            dishName: "New Dish",
            dishPicture: "a70a6112-964d-4f87-8853-0ad44b6d4a3a.png" // default dish image
        };
        console.warn("Adding new dish:", currentActiveMenu);
        const dish = await dishService.createDish(newDish, currentActiveMenu);
        cacheHandlerActions.addDish(currentActiveMenu, dish);
        AdminState.find((x: { menuId: string; }) => x.menuId === currentActiveMenu)?.dishes.push(dish);
        console.log(currentActiveMenu);
    }
	async function onClickMenuHandler(event: MouseEvent & { currentTarget: EventTarget & HTMLDivElement; }) {
        event.stopPropagation();
        //await UpdatePageData();
	}
    if(assignHandler)
        assignHandler(DBUpdatePageData);

</script>

<div class="admin-wrapper">
    <h2>Admin - Edit Menus</h2>
    
    <button class="create-menu-btn" onclick={createNewMenu}>+ Create New Menu</button>
    
    {#if menus && menus.length > 0}
        {#each AdminState as menu}
            <!-- svelte-ignore a11y_no_static_element_interactions -->
            <!-- svelte-ignore a11y_click_events_have_key_events -->
            <div onclick={async (event) => { currentActiveMenu = menu.menuId!; await onClickMenuHandler(event)}} class="RestMenuWrapper">
                <div class="row">
                    <RestMenu 
                        bind:name = {menu.menuName}
                        bind:theme = {menu.theme}
                        isEditMode={true}
                    />
                    <button  type="button" class="remove" onclick={async (e)=> {
                        e.stopImmediatePropagation();
                        let res = await menuService.deleteMenu(menu.menuId!);
                       // if(!res.success)
                       // {
                       //     throw Error(`DB remove menu failed! message: ${res.message} \n data: ${res.data}`)
                       //     return;
                       // }

                        cacheHandlerActions.removeMenu(menu.menuId!);
                        const curr = AdminState.findIndex((x: { menuId: any; }) => x.menuId == menu.menuId)

                        if(curr != -1)
                            AdminState.splice(curr, 1)
                    
                }}>-</button>
                </div>
                <div class="column">
                    <RestDish 
                    bind:dishes   = {menu.dishes}
                    bind:menuId   = {menu.menuId}
                    active   = {currentActiveMenu === menu.menuId}
                    edit     = {true}
                    />
                    <button class="add-dish-btn" onclick={async () => await addDish()}>+ Add Dish</button>
                </div>
            </div>
        {/each}
    {/if}
    <p>No menus available</p>
</div>

<style>
    .row{
        display: flex;
        flex-direction: row;
    }
   .RestMenuWrapper{
        overflow-y: scroll;
        background-color: sandybrown;
    } 
    .create-menu-btn {
        margin-bottom: 1rem;
        padding: 0.75rem 1.5rem;
        background: #4CAF50;
        color: white;
        border: none;
        cursor: pointer;
        border-radius: 4px;
        font-size: 1rem;
        font-weight: bold;
    }
    
    .create-menu-btn:hover {
        background: #45a049;
    }
    .admin-wrapper {
		display: flex;
        flex-direction: column;
		overflow-y: scroll;
		background: #f4f4f4;
        overflow-y: scroll;
	}
</style>

