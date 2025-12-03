<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from "$lib/services/MenuService";

    let { menus, currentMenu } = $props<{ menus: Menu[], currentMenu: Menu | null }>();
</script>

<div>
    <h2>All Menus</h2>
    {#if menus && menus.length > 0}
        {#each menus as menu}
            <RestMenu 
                menuItem={menu}
                isEditMode={false}
                remove={() => {}}
                selectedCB={() => {}}
                children={undefined}
            />
        {/each}
    {:else}
        <p>Inga Menyer finns</p>
    {/if}
</div>
