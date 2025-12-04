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
    const dishService = new DishService(apiService)

    
    let { menus = $bindable(), currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();
    
    let currentActiveMenu = $state("-1")
    let AdminState = $state(menus);

   async function createNewMenu(event : MouseEvent){

        event.stopImmediatePropagation()
        const newMenu: Menu = {
            menuId: "0" , // Temporary ID// was this a string?
            menuName: "Defaultmenu",
            userName: "Defautuser",
            theme: "menu-5507525_640.webp",
            dishes: []
        };

       const menu = await menuService.createMenu(newMenu);
       cacheHandlerActions.addMenu(menu);
       AdminState.push(menu)

    }

    function onClickDelete(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
    
	}
    async function addDish() {
        const newDish: Dish = {
            Id: 0, // Temporary ID
            DishName: "New Dish",
            DishPicture: "taco-8029161_640.png"
        };
        console.warn("Adding new dish:", currentActiveMenu);
        const dish = await dishService.createDish(newDish, currentActiveMenu);
        cacheHandlerActions.addDish(currentActiveMenu, dish);
        AdminState.filter((x,y) => x.menuId == currentActiveMenu)[0].dishes.push(dish)
        console.log(currentActiveMenu);
    }
	function onClickMenuHandler(event: MouseEvent & { currentTarget: EventTarget & HTMLDivElement; }) {
        event.stopPropagation();
	}
    function createDish() {
        return  {
            Id: 0,
            DishName: "defaultDish",

            DishPicture: "taco-8029161_640.png"
        } as Dish
    }
</script>

<div class="admin-wrapper">
    <h2>Admin - Edit Menus</h2>
    
    <button class="create-menu-btn" onclick={createNewMenu}>+ Create New Menu</button>
    
    {#if menus && menus.length > 0}
        {#each AdminState as menu}
            <!-- svelte-ignore a11y_no_static_element_interactions -->
            <!-- svelte-ignore a11y_click_events_have_key_events -->
            <div onclick={(event) => { currentActiveMenu = menu.menuId; onClickMenuHandler(event)}} class="RestMenuWrapper">
                <div class="row">
                    <RestMenu 
                    menuItem={menu}
                    isEditMode={true}
                    />
                    <button  type="button" class="remove" onclick={(e)=> {
                        e.stopImmediatePropagation();
                        cacheHandlerActions.removeMenu(menu.menuId);
                        const curr = AdminState.findIndex((x,y) => x.menuId == menu.menuId)

                        if(curr != -1)
                            AdminState.splice(curr, 1)
                    
                }}>-</button>
                </div>
                <div class="column">
                    <RestDish 
                    dishes   = {menu.dishes ?? []}
                    menuId   = {menu.menuId}
                    active   = {currentActiveMenu === menu.menuId}
                    edit     = {true}
                        children = {undefined}/>
                
                    <button class="add-dish-btn" onclick={async () => await addDish()}>+ Add Dish</button>
                </div>
            </div>
        {/each}
    {/if}
    <p>No menus available</p>
</div>

<style>
    .row{

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

