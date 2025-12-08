<script lang="ts">
    import  RestMenu from "../RestMenu.svelte";
    import type { Menu } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { menus, currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();
    let selectedMenu = $state(currentMenu);
    menus = cacheHandlerActions.getActiveCache().menus;

    function OnSelectedMenu(){
        console.log("************SETMENU**********") 
        console.log(selectedMenu)
        console.log("************SETMENU**********") 
        cacheHandlerActions.setCurrentMenu(selectedMenu)
    }

</script>

<div>
    <h2>All Menus {menus.length}</h2>
    {#if menus && menus.length > 0}
        {#each menus as menu}
            <RestMenu 
                name={menu.menuName}
                theme={menu.theme}
                isEditMode={false}
                />
            <button type="button" class="pickmeny" onclick={() => cacheHandlerActions.setCurrentMenu(menu)}> Välj meny</button>
        {/each}
    {:else}
        <p>Inga Menyer finns</p>
    {/if}
</div>
<style>
.pickmeny {
        background: rgb(155, 115, 6);
        color: rgb(238, 238, 238);
        border: none;
        padding: 0.5rem 1rem;
        cursor: pointer;
        border-radius: 4px;
    }
</style>
