<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import { ApiService } from "$lib/services/apiService";
    import { MenuService } from "$lib/services/MenuService";
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";
	import { stopImmediatePropagation } from "svelte/legacy";
	import RestDish from "../RestDish.svelte";
	import { DishService, type Dish } from "$lib/services/DishService";
    import { validateField, type IValidationResult } from "$lib/Validations/clientValidations";
    
    const apiService = new ApiService();
    const menuService = new MenuService(apiService);
    const dishService = new DishService(apiService);
    
    let { menus,
        currentMenu,
        assignHandler } = $props<{ menus: Menu[],
            currentMenu: Menu | null,
            assignHandler: (handleSwap: () => Promise<void>) => void}>();

        let currentActiveMenu :string = $state("-1")
        let expandedMenuId: string | null = $state(null);
        let AdminState: Menu[] = $state(menus)

        //let ChangedValue: string[] = $state([])
        //AdminState = cacheHandlerActions.getActiveCache().menus;
            
                
                //exports removed, they not needed
const DBUpdatePageData = () => {
    updateMenus();
    for(let i = 0; i < AdminState.length; i++)
    updateDishes(AdminState[i])
}
    

    async function updateMenus(){
        let currMen :Menu[] = cacheHandlerActions.getActiveCache().menus
        for(let i = 0; i < currMen.length; i ++)
        //dropping into javascript cus lazy, should be a helper to compare the fields
            if(!Object.keys(currMen[i]).map((x) => { 
                if(x !== 'dishes') 
                    return AdminState[i][x] === currMen[i][x] //work this will not when smarter the compiler gets
                else return true;
             }).every(x => x))
            {
                cacheHandlerActions.updateMenu(AdminState[i])
            }
    }
    async function updateDishes(menu: Menu){
        //the aforementioned local helper
        const evalElem = (cacheDish :Dish, adminDish : Dish) :boolean =>{
            return (cacheDish.dishName === adminDish.dishName 
                && cacheDish.dishPicture === adminDish.dishPicture)
        };
        //reference for the cache object
        const cacheDishes = cacheHandlerActions.getActiveCache().menus.find((x) => x.menuId == menu.menuId)

        if(!cacheDishes)
            throw Error(`couldnt locate: ${menu.menuId} in cache, state is disjointed`)

        //loop over, compare, update
        for(let i = 0; i < menu.dishes.length; i++)
            if(!evalElem(cacheDishes.dishes[i], menu.dishes[i])){
                cacheHandlerActions.updateDish(menu.menuId!, menu.dishes[i])
            }
    }

    

    function toggleMenu(menuId: string) {
        if (expandedMenuId === menuId) {
            expandedMenuId = null;
            currentActiveMenu = "-1";
        } else {
            expandedMenuId = menuId;
            currentActiveMenu = menuId;
        }
    }
    function handleDishNameValueChange(value :string, dish: Dish){
        const validation = validateField.validateDishName(value)
        
        return validation.value;
    }

    function isMenuExpanded(menuId: string): boolean {
        return expandedMenuId === menuId;
    }

   async function createNewMenu(event : MouseEvent){

        event.stopImmediatePropagation()
        const newMenu: Menu = {
            menuId: "0" , // Temporary ID // Why is this a string? Why not number?
            menuName: "Ny meny",
            userName: "Defautuser",
            theme: "d1bbc886-0a27-4f95-9bf1-7ed9758694c7", // Default theme image
            dishes: []
        };
    
       const menu = await menuService.createMenu(newMenu);
       if(menu.menuId != null){
           menu.menuId = menu.menuId.toString();
   
          cacheHandlerActions.addMenu(menu);
          AdminState.push(menu)
       }
    }

    function onClickDelete(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
    
	}

    async function addDish() {
        const newDish: Dish = {
            id: 0, // Temporary ID
            dishName: "Ny rätt",
            dishPicture: "a70a6112-964d-4f87-8853-0ad44b6d4a3a" // Default dish image
        };
        console.warn("Adding new dish:", currentActiveMenu);
        const dish = await dishService.createDish(newDish, currentActiveMenu);
        cacheHandlerActions.addDish(currentActiveMenu, dish);
        //AdminState = cacheHandlerActions.getActiveCache().menus;

         AdminState.find(x => x.menuId === currentActiveMenu)?.dishes.push(dish);

        console.log(currentActiveMenu);
    }
	async function onClickMenuHandler(event: MouseEvent & { currentTarget: EventTarget & HTMLDivElement; }) {
        event.stopPropagation();
        //await UpdatePageData();
	}
    if(assignHandler)
        assignHandler(DBUpdatePageData);

</script>

<div class="admin-container">
    <h2 class="admin-title">⚙️ Admin ⚙️</h2>
    
    <button class="create-menu-btn" onclick={createNewMenu}>
        <span class="btn-icon"> + </span>
        Skapa Ny Meny
    </button>
    
    {#if menus && menus.length > 0}
        <div class="menus-list">
            {#each AdminState as menu}
                <div class="menu-item" class:expanded={isMenuExpanded(menu.menuId!)} class:bounce={!validateField.validateMenuName(menu.menuName).valid}>
                    <!-- svelte-ignore a11y_click_events_have_key_events -->
                    <!-- svelte-ignore a11y_no_static_element_interactions -->
                    <div class="menu-card" onclick={() => toggleMenu(menu.menuId!)}>
                        <div class="menu-image-container">
                            <img 
                                src="/pictures/{menu.theme}" 
                                alt={menu.menuName}
                                class="menu-image"
                            />
                        </div>

                        <div class="menu-info-container">
                            <textarea 
                                bind:value={() => menu.menuName,
                                            (v) => menu.menuName = validateField.validateMenuName(v).value}
                                class="menu-name-input"
                                onclick={(e) => e.stopPropagation()}
                                placeholder="Menyns namn"
                                rows="2"
                            ></textarea>

                            <div class="expand-hint">
                                {#if isMenuExpanded(menu.menuId!)}
                                    ▲ Stäng
                                {:else}
                                    ▼ Öppna
                                {/if}
                            </div>
                        </div>
                    </div>

                    {#if isMenuExpanded(menu.menuId!)}
                        <div class="dishes-dropdown">
                            <div class="control-buttons">
                                <button 
                                    class={["btn-add-dish"]}
                                    onclick={async () => await addDish()}
                                >
                                    <span class="btn-icon">+</span>
                                    Lägg till Rätt
                                </button>
                                
                                <button 
                                    class="btn-delete-menu"
                                    onclick={async () => {
                                        let res = await menuService.deleteMenu(menu.menuId!);
                                        cacheHandlerActions.removeMenu(menu.menuId!);
                                        const curr = AdminState.findIndex((x) => x.menuId == menu.menuId)
                                        if(curr != -1)
                                            AdminState.splice(curr, 1)
                                    }}
                                >
                                    <span class="btn-icon">×</span>
                                    Radera Meny
                                </button>
                            </div>

                            <div class="dishes-list">
                                {#if menu.dishes && menu.dishes.length > 0}
                                    {#each menu.dishes as dish}
                                        <div class={["dish-card"]} class:bounce={!validateField.validateDishName(dish.dishName).valid} id={dish.id?.toString()}>
                                            <div class="dish-image-container">
                                                <img 
                                                    src="/pictures/{dish.dishPicture}" 
                                                    alt={dish.dishName}
                                                    class="dish-image"
                                                />
                                            </div>

                                            <div class="dish-name-container" >
                                                <textarea 
                                                    bind:value={() => dish.dishName,
                                                                (v) => dish.dishName = validateField.validateDishName(v).value}
                                                    class="dish-name-input"
                                                    placeholder="Rättens namn"
                                                    rows="2"
                                                ></textarea>
                                            </div>

                                            <div class="dish-delete-container">
                                                <button 
                                                    class="btn-delete-dish"
                                                    onclick={async () => {
                                                        const dishIndex = menu.dishes.findIndex(x => x.id == dish.id);
                                                        if(dishIndex == -1) throw new Error("Dish not found");
                                                        menu.dishes.splice(dishIndex, 1);
                                                        cacheHandlerActions.removeDish(menu.menuId!, dish.id!);
                                                        await dishService.deleteDish(dish.id!);
                                                    }}
                                                > × </button>
                                            </div>
                                        </div>
                                    {/each}
                                {:else}
                                    <p class="no-dishes">Inga rätter ännu. Lägg till en!</p>
                                {/if}
                            </div>
                        </div>
                    {/if}
                </div>
            {/each}
        </div>
    {:else}
        <p class="no-menus">Inga menyer finns. Skapa en!</p>
    {/if}
</div>

<style>
    .admin-container {
        padding: 1rem;
    }
    
    .admin-title {
        color: #9C27B0;
        font-size: 2.2rem;
        font-weight: bold;
        margin-bottom: 1.5rem;
        text-align: center;
    }

    .create-menu-btn {
        width: 100%;
        background: linear-gradient(135deg, #a8ff78 0%, #00CED1 100%);
        color: white;
        border: 2px solid #00A8AA;
        padding: 1.2rem 2rem;
        cursor: pointer;
        border-radius: 16px;
        font-size: 1.5rem;
        font-weight: bold;
        margin-bottom: 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 0.5rem;
        box-shadow: 0 6px 12px rgba(0, 206, 209, 0.4);
        transition: all 0.3s ease;
        text-shadow: 1px 1px 2px #00A8AA, -1px -1px 2px #00A8AA, 1px -1px 2px #00A8AA, -1px 1px 2px #00A8AA;
    }
    
    .create-menu-btn:hover {
        transform: scale(1.05);
        box-shadow: 0 8px 16px rgba(0, 206, 209, 0.5);
    }
    
    .btn-icon {
        font-size: 2rem;
        line-height: 1;
    }

    /* Handles the styling for the menu cards in the list of menues */
    .menus-list {
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
    }

    .menu-item {
        background: white;
        border-radius: 16px;
        overflow: hidden;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
        border: 2px solid #9C27B0;
    }
    
    .menu-item.expanded {
        box-shadow: 0 8px 20px rgba(156, 39, 176, 0.3);
    }

    .menu-card {
        display: flex;
        flex-direction: row;
        height: 120px;
        cursor: pointer;
        transition: all 0.2s ease;
    }
    
    .menu-card:hover {
    }

    .menu-image-container {
        flex: 1;
        min-width: 0;
    }
    
    .menu-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .menu-info-container {
        flex: 1;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        align-items: center;
        padding: 0.75rem;
    }
    
    .menu-name-input {
        width: 100%;
        font-size: 1.3rem;
        font-weight: bold;
        color: #333;
        text-align: center;
        border: 2px dashed transparent;
        background: transparent;
        padding: 0.5rem;
        border-radius: 8px;
        transition: all 0.2s ease;
        resize: none;
        overflow: hidden;
        font-family: system-ui;
        line-height: 1.3;
    }
    
    .menu-name-input:hover {
        border-color: #9C27B0;
        background: #f5f5f5;
    }
    
    .menu-name-input:focus {
        outline: none;
        border-color: #9C27B0;
        background: white;
        box-shadow: 0 0 0 3px rgba(156, 39, 176, 0.1);
    }
    
    .expand-hint {
        font-size: 0.85rem;
        color: #9C27B0;
        font-weight: bold;
        margin-top: auto;
        padding-top: 0.25rem;
    }

    .dishes-dropdown {
        padding: 1rem;
        background: #f9f3fc;
        border-top: 3px solid #9C27B0;
    }

    .control-buttons {
        display: flex;
        gap: 1rem;
        margin-bottom: 1.5rem;
    }
    
    .btn-add-dish {
        flex: 1;
        background: linear-gradient(135deg, #a8ff78 0%, #00CED1 100%);
        color: white;
        border: 2px solid #00A8AA;
        padding: 1rem 1.5rem;
        cursor: pointer;
        border-radius: 12px;
        font-size: 1.3rem;
        font-weight: bold;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 0.5rem;
        box-shadow: 0 4px 8px rgba(0, 206, 209, 0.4);
        transition: all 0.2s ease;
        text-shadow: 1px 1px 2px #00A8AA, -1px -1px 2px #00A8AA, 1px -1px 2px #00A8AA, -1px 1px 2px #00A8AA;
    }
    
    .btn-add-dish:hover {
        transform: scale(1.05);
        box-shadow: 0 6px 12px rgba(0, 206, 209, 0.5);
    }
    
    .btn-delete-menu {
        flex: 1;
        background: linear-gradient(135deg, #FC6675 0%, #AB537C 100%);
        color: white;
        border: 2px solid #8B2F5C;
        padding: 1rem 1.5rem;
        cursor: pointer;
        border-radius: 12px;
        font-size: 1.3rem;
        font-weight: bold;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 0.5rem;
        box-shadow: 0 4px 8px rgba(252, 102, 117, 0.4);
        transition: all 0.2s ease;
        text-shadow: 1px 1px 2px #8B2F5C, -1px -1px 2px #8B2F5C, 1px -1px 2px #8B2F5C, -1px 1px 2px #8B2F5C;
    }
    
    .btn-delete-menu:hover {
        transform: scale(1.05);
        box-shadow: 0 6px 12px rgba(252, 102, 117, 0.5);
    }

    /* Handles the styling for the dish cards in the list of dishes for each menu */
    .dishes-list {
        display: flex;
        flex-direction: column;
        gap: 0.75rem;
    }

    .dish-card {
        display: flex;
        flex-direction: row;
        background: white;
        border-radius: 12px;
        overflow: hidden;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        height: 80px;
    }

    .dish-image-container {
        flex: 0 0 25%;
    }
    
    .dish-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .dish-name-container {
        flex: 0 0 50%;
        display: flex;
        align-items: center;
        padding: 0.5rem;
    }
    
    .dish-name-input {
        width: 100%;
        font-size: 1.1rem;
        font-weight: bold;
        color: #333;
        text-align: center;
        border: 2px dashed transparent;
        background: transparent;
        padding: 0.5rem;
        border-radius: 6px;
        transition: all 0.2s ease;
        resize: none;
        overflow: hidden;
        font-family: system-ui;
        line-height: 1.3;
    }
    
    .dish-name-input:hover {
        border-color: #9C27B0;
        background: #f5f5f5;
    }
    
    .dish-name-input:focus {
        outline: none;
        border-color: #9C27B0;
        background: white;
        box-shadow: 0 0 0 2px rgba(156, 39, 176, 0.1);
    }

    .dish-delete-container {
        flex: 0 0 25%;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 0.5rem;
    }
    
    .btn-delete-dish {
        width: 50px;
        height: 50px;
        background: linear-gradient(135deg, #FC6675 0%, #AB537C 100%);
        color: white;
        border: 2px solid #8B2F5C;
        border-radius: 10px;
        font-size: 2rem;
        font-weight: bold;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 3px 6px rgba(252, 102, 117, 0.4);
        transition: all 0.2s ease;
        line-height: 1;
        text-shadow: 1px 1px 2px #8B2F5C, -1px -1px 2px #8B2F5C, 1px -1px 2px #8B2F5C, -1px 1px 2px #8B2F5C;
    }
    
    .btn-delete-dish:hover {
        transform: scale(1.1);
        box-shadow: 0 4px 8px rgba(252, 102, 117, 0.5);
    }
    
    .btn-delete-dish:active {
        transform: scale(0.95);
    }

    .no-dishes {
        text-align: center;
        color: #9C27B0;
        font-size: 1.2rem;
        padding: 2rem;
        font-style: italic;
    }
    
    .no-menus {
        text-align: center;
        color: #888;
        font-size: 1.5rem;
        margin-top: 3rem;
    }
    .bounce {
        background-color: #FC6675 !important;
        animation: bounce 1s ease infinite;

        }

    @keyframes bounce {

    70% { transform:translateY(0%); }

    80% { transform:translateY(-35%); }

    90% { transform:translateY(0%); }

    95% { transform:translateY(-23%); }

    97% { transform:translateY(0%); }

    99% { transform:translateY(-12%); }

    100% { transform:translateY(0); }

}
</style>