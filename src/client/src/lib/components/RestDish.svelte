<script lang="ts">
    import { type Dish } from "$lib/services/DishService";
	import { cacheHandlerActions } from "../../stores/cacheHandlerService";
    import { DishService } from "$lib/services/DishService";
    import { ApiService } from "$lib/services/apiService";
    
    const apiService = new ApiService();
    const dishService = new DishService(apiService);
    
    let {
        dishes = $bindable([]), 
        active,
        edit,
        children,
        menuId
    } = $props()
    
    // Track quantity for each dish (for non-edit mode)
    let quantities = $state<Record<number, number>>({});

    function onImageClick(dishId: number) {
        // TODO: Add image upload/change functionality
        console.log('Image clicked for dish', dishId);
    }

    function onClickDelete(dishId: number) {
        dishes = dishes.filter(d => d.id !== dishId);
        cacheHandlerActions.removeDish(menuId, dishId)
    }

    async function addDish() {
        const newDish: Dish = {
            id: null, // Temporary ID
            name: "New Dish",
            foodPicture: ""
        };
        const dish = await dishService.createDish(newDish, menuId);
        cacheHandlerActions.addDish(menuId, dish);
        
    }

    function incrementQuantity(dishId: number) {
        if (!quantities[dishId]) {
            quantities[dishId] = 0;
        }
        quantities[dishId]++;
    }

    function decrementQuantity(dishId: number) {
        if (quantities[dishId] && quantities[dishId] > 0) {
            quantities[dishId]--;
        }
    }

    function getQuantity(dishId: number): number {
        return quantities[dishId] || 0;
    }
</script>
<div>
    {#if active}
        {#each dishes as dish}
            {#if !edit}
                <!-- View mode: show dish with +/- quantity controls -->
                <div class='row'>
                    <img src={dish.foodPicture} alt={dish.name} class="dish-image" />
                    {#if dish.name.length > 0}
                    <p class="dish-name">{dish.name}</p>
                    {:else}
                    <p class="dish-name">Inge namn på rätt</p>
                    {/if}
                    <div class="quantity-controls">
                        <button class="quantity-btn" onclick={() => decrementQuantity(dish.id)}>-</button>
                        <span>{getQuantity(dish.id)}</span>
                        <button class="quantity-btn" onclick={() => incrementQuantity(dish.id)}>+</button>
                    </div>
                    {@render children?.()}
                </div>
            {:else}
                <!-- Edit mode: show dish with editable name, image, and remove button -->
                <div class='row'>
                    <img 
                        src={dish.foodPicture.length > 0 ? dish.foodPicture : '/pictures/taco-8029161_640.png'}
                        alt={dish.name} 
                        class="dish-image" 
                        onclick={() => onImageClick(dish.id)}
                        style="cursor: pointer;"
                    />
                    <input type="text" bind:value={dish.name} class="dish-name" placeholder="Dish name" />
                    <button class="remove-btn" onclick={() => onClickDelete(dish.id)}>Remove</button>
                </div>
            {/if}
        {/each}
        
        {#if edit}
        <button class="add-dish-btn" onclick={() => addDish()}>+ Add Dish</button>
        {/if}
    {/if}
</div>
<style>
    .row {
        display: flex;
        flex-direction: row;
        align-items: center;
        gap: 1rem;
        margin-bottom: 0.5rem;
        padding: 0.5rem;
        border: 1px solid #ddd;
        border-radius: 4px;
    }
    
    .dish-image {
        width: 80px;
        height: 80px;
        object-fit: cover;
        border-radius: 4px;
    }
    
    .dish-name {
        flex: 1;
        margin: 0;
    }
    
    .quantity-controls {
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }
    
    .quantity-btn {
        width: 30px;
        height: 30px;
        border: 1px solid #333;
        background: white;
        cursor: pointer;
        border-radius: 4px;
    }
    
    .quantity-btn:hover {
        background: #f0f0f0;
    }
    
    .remove-btn {
        background: #ff4444;
        color: white;
        border: none;
        padding: 0.5rem 1rem;
        cursor: pointer;
        border-radius: 4px;
    }
    
    .remove-btn:hover {
        background: #cc0000;
    }


    .add-dish-btn {
        margin-top: 1rem;
        padding: 0.5rem 1rem;
        background: #4CAF50;
        color: white;
        border: none;
        cursor: pointer;
        border-radius: 4px;
    }
    
    .add-dish-btn:hover {
        background: #45a049;
    }
</style>

