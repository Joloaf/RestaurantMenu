<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';

    let { currentMenu } = $props<{ currentMenu: Menu | null }>();
</script>

<div>
    <h2>Current Menu {currentMenu.name}</h2>
    {#if currentMenu}
        <RestMenu 
            menuItem={currentMenu}
            isEditMode={false}
            remove={() => {}}
            selectedCB={() => {}}
            children={undefined}
        />
    {:else}
        <p>No menu selected</p>
    {/if}
</div>
