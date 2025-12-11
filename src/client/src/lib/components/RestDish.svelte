<script lang="ts">
    import { type Dish } from "$lib/services/DishService";
	import { cacheHandlerActions } from "../../stores/cacheHandlerService";
    import { DishService } from "$lib/services/DishService";
    import { ApiService } from "$lib/services/apiService";
    
    const apiService = new ApiService();
    const dishService = new DishService(apiService);
    const picturePath = "/static/pictures/"
    
    let {
        dishes = $bindable(),
        menuId = $bindable(),
        active,
        edit,
    } = $props()

    let dishesBound : Dish[] = $derived(dishes)
    // Track quantity for each dish (for non-edit mode)
    let quantities = $state<Record<number, number>>({});


    function onImageClick(dishId: number) {
        // TODO: Add image upload/change functionality
        console.log('Image clicked for dish', dishId);
    }

    async function onClickDelete(dishId: number) {
        //another one of those, "this will not work when the compiler gets smarter"
        const dishIndex = dishesBound.findIndex(x => x.id == dishId);
        
        if(dishIndex == -1)
        throw new Error("I physically cannot");
    
        dishesBound.splice(dishIndex, 1); //intentional loose equality cus i'm not dealing with the type issues.
        cacheHandlerActions.removeDish(menuId, dishId);
        await dishService.deleteDish(dishId);
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
        {#each dishesBound as dish}
            {#if !edit}
                <!-- View mode: show dish with +/- quantity controls -->
                <div class='row'>
                    <img src={dish.dishPicture} alt={dish.dishName} class="dish-image" />
                    {#if dish.dishName.length > 0}
                    <p class="dish-name">{dish.dishName}</p>
                    {:else}
                    <p class="dish-name">Inge namn på rätt</p>
                    {/if}
                    <div class="quantity-controls">
                        <button class="quantity-btn" onclick={() => decrementQuantity(dish.id!)}>-</button>
                        <span>{getQuantity(dish.id!)}</span>
                        <button class="quantity-btn" onclick={() => incrementQuantity(dish.id!)}>+</button>
                    </div>
                </div>
            {:else}
                <!-- Edit mode: show dish with editable name, image, and remove button -->
                <div class='row' style="background-image:url('{picturePath}{dishes.theme}' ">
                    <img 
                        src={(dish.dishPicture?.length ?? -1 ) > 0 ? '/pictures/'+dish.dishPicture : '/pictures/a70a6112-964d-4f87-8853-0ad44b6d4a3a'}
                        alt={dish.dishName} 
                        class="dish-image" 
                        onclick={() => onImageClick(dish.id!)}
                        style="cursor: pointer;"
                    />
                    <input type="text" bind:value={dish.dishName} class="dish-name" placeholder="Dish name" />
                    <button class="remove-btn" onclick={async () => await onClickDelete(dish.id!)}>Remove</button>
                </div>
            {/if}
        {/each}
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

