<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import RestDish from "../RestDish.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { currentMenu } = $props<{ currentMenu: Menu | null }>();
    currentMenu = cacheHandlerActions.getActiveCache().currentMenu;
</script>

<div>
    <h2>Current Menu {currentMenu.name}</h2>
    {#if currentMenu}
        <RestMenu 
            menuItem={currentMenu}
            isEditMode={false}
            />
                    <RestDish 
                    dishes   = {currentMenu.dishes ?? []}
                    menuId   = {currentMenu.menuId}
                    active   = {currentActiveMenu === menu.menuId}
                    edit     = {true}
                        children = {undefined}/>
    {:else}
        <p>No menu selected</p>
    {/if}
</div>
