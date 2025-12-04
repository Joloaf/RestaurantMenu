<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import { ApiService } from "$lib/services/apiService";
    import { MenuService } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";
	import { stopImmediatePropagation } from "svelte/legacy";
	import RestDish from "../RestDish.svelte";

    const apiService = new ApiService();
    const menuService = new MenuService(apiService);
    let currentActiveMenu = $state(-1)

    let { menus, currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();
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

        //
        // if(menu.CreationStamp != null)
        //  -> add to cache cus then you know it needs to be added, right now it looks like 
        //  

        cacheHandlerActions.addMenu(menu);

    }


	function onClickMenuHandler(event: MouseEvent & { currentTarget: EventTarget & HTMLDivElement; }) {
        event.stopPropagation();
	}
</script>

<div class="admin-wrapper">
    <h2>Admin - Edit Menus</h2>
    
    <button class="create-menu-btn" onclick={createNewMenu}>+ Create New Menu</button>
    
    {#if menus && menus.length > 0}
        {#each menus as menu}
            <button onclick={(event) => { currentActiveMenu = menu.menuId; onClickMenuHandler(event)}} class="RestMenuWrapper">
                <RestMenu 
                menuItem={menu}
                isEditMode={true}
                remove={() => {}}
                selectedCB={() => {}}
                children={undefined}
                />
                <div class="column">
                    <RestDish 
                        dishes   = {menu.dishes ?? []}
                        menuId   = {menu.menuId}
                        active   = {currentActiveMenu === menu.menuId}
                        edit     = {true}
                        children = {undefined}/>
                </div>
            </button>
        {/each}
    {/if}
    <p>No menus available</p>
</div>

<style>
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

