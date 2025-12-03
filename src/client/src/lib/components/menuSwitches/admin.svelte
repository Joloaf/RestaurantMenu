<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import { ApiService } from "$lib/services/apiService";
    import { MenuService } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    
    const apiService = new ApiService();
    const menuService = new MenuService(apiService);

    let { menus, currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();

   async function createNewMenu() {
        const newMenu: Menu = {
            menuId: null, // Temporary ID
            menuName: "",
            userName: "",
            theme: "",
            dishes: []
        };
        const menu = await menuService.createMenu(newMenu);
        
        cacheHandlerActions.addMenu(menu);

    }
</script>

<div class="admin-wrapper">
    <h2>Admin - Edit Menus</h2>
    
    <button class="create-menu-btn" onclick={createNewMenu}>+ Create New Menu</button>
    
    {#if menus && menus.length > 0}
        {#each menus as menu}
            <RestMenu 
                menuItem={menu}
                isEditMode={true}
                remove={() => {}}
                selectedCB={() => {}}
                children={undefined}
            />
        {/each}
    {:else}
        <p>No menus available</p>
    {/if}
</div>

<style>
    
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

