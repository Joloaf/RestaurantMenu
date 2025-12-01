<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';

    let { menus, currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();

    function createNewMenu() {
        const newMenu: Menu = {
            menuId: Date.now(), // Temporary ID
            menuName: "New Menu",
            userName: "",
            theme: "",
            userId: "",
            dishes: []
        };
        menus.push(newMenu);
        // TODO: Call API to create menu and update cache
    }
</script>

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
</style>

<div>
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
