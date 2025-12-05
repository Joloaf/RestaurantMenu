<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { menus, currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();

    menus = cacheHandlerActions.getActiveCache().menus;

</script>

<div>
    <h2>All Menus {menus.length}</h2>
    {#if menus && menus.length > 0}
        {#each menus as menu}
            <RestMenu 
                menuItem={currentMenu}
                isEditMode={false}
                active= {currentMenu.id === menu.id}
            />
        {/each}
    {:else}
        <p>Inga Menyer finns</p>
    {/if}
</div>
