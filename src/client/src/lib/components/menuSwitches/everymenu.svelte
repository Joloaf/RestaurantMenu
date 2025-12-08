<script lang="ts">
    import  RestMenu from "../RestMenu.svelte";
    import type { Menu } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { menus, currentMenu = $bindable() } = $props<{ menus: Menu[], currentMenu: Menu | null }>();
    
    // Making good use of our cache to ensure we have nothing stale in this restaurant menu!
    menus = cacheHandlerActions.getActiveCache().menus;
    currentMenu = cacheHandlerActions.getActiveCache().currentMenu;
    
    // Local states for selected menu badge and animation
    let selectedMenuId = $state(currentMenu?.menuId || null);
    let justSelected = $state(false);

    function isActiveMenu(menu: Menu): boolean {
        return selectedMenuId === menu.menuId;
    }

    function selectMenu(menu: Menu) {
        cacheHandlerActions.setCurrentMenu(menu);   // Cache update
        selectedMenuId = menu.menuId!;              // Local state update
        currentMenu = menu;                         // Making sure the parent(s) are aware!
        
        // Flash animation for 1 sec
        justSelected = true;
        setTimeout(() => {
            justSelected = false;
        }, 1000);
    }
</script>

<div class="everymenu-container">
    <h2 class="title">Alla Menyer ({menus.length})</h2>
    
    {#if menus && menus.length > 0}
        <div class="menus-list">
            {#each menus as menu}
                <div class="menu-card" 
                     class:active={isActiveMenu(menu)}
                     class:flash={isActiveMenu(menu) && justSelected}>
                    {#if isActiveMenu(menu)}
                        <div class="active-badge">
                            <span class="star">⭐</span>
                            <span>AKTIV MENY</span>
                        </div>
                    {/if}
                    
                    <RestMenu 
                        name={menu.menuName}
                        theme={menu.theme}
                        isEditMode={false}
                    />
                    
                    <button 
                        type="button" 
                        class="select-btn"
                        class:selected={isActiveMenu(menu)}
                        disabled={isActiveMenu(menu)}
                        onclick={() => selectMenu(menu)}
                    >
                        {#if isActiveMenu(menu)}
                            VALD
                        {:else}
                            Välj meny
                        {/if}
                    </button>
                </div>
            {/each}
        </div>
    {:else}
        <p class="no-menus">Inga Menyer finns</p>
    {/if}
</div>

<style>
    .everymenu-container {
        padding: 1rem;
    }
    
    .title {
        color: #FF6B6B;
        font-size: 2.5rem;
        font-weight: bold;
        margin-bottom: 1.5rem;
        text-align: center;
    }
    
    .menus-list {
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
    }

    /* Handles the styling for the menu cards in the list of menues */
    .menu-card {
        background: white;
        border-radius: 16px;
        padding: 1.5rem;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
        border: 3px solid transparent;
        position: relative;
    }
    
    .menu-card:hover:not(.active) {
        transform: translateY(-2px);
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
    }
    
    .menu-card.active {
        border: 5px solid #4ECDC4;
        background: linear-gradient(135deg, #FFF9E6 0%, #FFE5F1 100%);
        box-shadow: 0 8px 20px rgba(78, 205, 196, 0.4);
    }

    .menu-card.flash {
        animation: flashHighlight 0.8s ease-out;
    }

    /* Keyframes to animate the flash effect when selecting a new active menu */
    @keyframes flashHighlight {
        0% {
            box-shadow: 0 0 0 0 rgba(78, 205, 196, 0.8);
            transform: scale(1);
        }
        25% {
            box-shadow: 0 0 30px 15px rgba(78, 205, 196, 0.6);
            transform: scale(1.03);
        }
        50% {
            box-shadow: 0 0 40px 20px rgba(255, 107, 107, 0.4);
            transform: scale(1.05);
        }
        75% {
            box-shadow: 0 0 30px 15px rgba(78, 205, 196, 0.6);
            transform: scale(1.03);
        }
        100% {
            box-shadow: 0 8px 20px rgba(78, 205, 196, 0.4);
            transform: scale(1);
        }
    }

    /* Handles the styling of the badge for active menu selection */
    .active-badge {
        position: absolute;
        top: -12px;
        right: 1rem;
        z-index: 10;
        background: linear-gradient(135deg, #4ECDC4 0%, #44A08D 100%);
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 20px;
        font-weight: bold;
        font-size: 0.9rem;
        display: flex;
        align-items: center;
        gap: 0.5rem;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        animation: pulse 2s ease-in-out infinite;
    }
    
    @keyframes pulse {
        0%, 100% {
            transform: scale(1);
        }
        50% {
            transform: scale(1.05);
        }
    }

    .star {
        font-size: 1.2rem;
        animation: rotateScale 4s linear infinite;
        display: inline-block;
    }

    @keyframes rotateScale {
        from {
            transform: scale(1.5) rotate(0deg);
        }
        to {
            transform: scale(1.5) rotate(360deg);
        }
    }

    /* Handles the styling of the select button for each menu */
    .select-btn {
        width: 100%;
        background: linear-gradient(135deg, #FF6B6B 0%, #FFB347 100%);
        color: white;
        border: none;
        padding: 1.5rem 2rem;
        cursor: pointer;
        border-radius: 16px;
        font-size: 1.6rem;
        font-weight: bold;
        margin-top: 0.5rem;
        transition: all 0.3s ease;
        box-shadow: 0 6px 12px rgba(255, 107, 107, 0.4);
        text-transform: uppercase;
    }
    
    .select-btn:hover:not(:disabled) {
        transform: scale(1.05);
        box-shadow: 0 8px 16px rgba(255, 107, 107, 0.5);
    }
    
    .select-btn:active:not(:disabled) {
        transform: scale(0.98);
    }
    
    .select-btn.selected {
        background: linear-gradient(135deg, #95E1D3 0%, #4ECDC4 100%);
        cursor: default;
        box-shadow: 0 6px 12px rgba(78, 205, 196, 0.4);
    }
    
    .select-btn:disabled {
        opacity: 0.9;
    }
    
    .no-menus {
        text-align: center;
        color: #888;
        font-size: 1.5rem;
        margin-top: 2rem;
    }
</style>
