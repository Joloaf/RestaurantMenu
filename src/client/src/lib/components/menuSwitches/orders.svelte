<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import RestDish from "../RestDish.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { currentMenu = cacheHandlerActions.getActiveCache().currentMenu } = $props<{ currentMenu: Menu | null }>();
</script>

<div>
    {#if currentMenu}
        <h2>Current Menu {currentMenu.name}</h2>
        <RestMenu 
            menuItem={currentMenu}
            isEditMode={false}
        />
        <RestDish
            dishes={currentMenu.dishes}
            menuId={currentMenu.menuId}
            active={true}
            edit={false} 
            children={true}
        />
    {:else}
        <p>No menu selected</p>
    {/if}
</div>