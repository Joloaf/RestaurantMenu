<script lang="ts">
    import RestMenu from "../RestMenu.svelte";
    import RestDish from "../RestDish.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import type { Ticket } from "../../../stores/cacheHandlerService"
    import { type Order} from "../../../stores/cacheHandlerService"
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { currentMenu } = $props<{ currentMenu: Menu | null }>();
    //bind to tickets
    let i = -1;
    let orders : Order = $state(populateOrder())

    function populateOrder() : Order{
        let i = -1;
        const orders :Order = { } as Order;
        orders.ticket = currentMenu.dishes.map(x => ({ dishes: x, id : i++, quantity: 0 } as Ticket))
        orders.menuId = currentMenu.menuId;
        orders.ticketNumber = 0;

        return orders;
    }
    function onClickOrder(){

    }
</script>

<div>
    {#if currentMenu}
        <h2>Current Menu {currentMenu.userName}</h2>
        <RestMenu 
            theme = {currentMenu.theme}
            name= {currentMenu.menuName}
            isEditMode={false}
        />
        {#each orders.ticket as ticket}
            <div>
                <img src={ticket.dishes?.dishPicture.length ?? -1 > 0 ? "/pictures/"+ticket.dishes.dishPicture : "/pictures/a70a6112-964d-4f87-8853-0ad44b6d4a3a.png"}/>
                <p>{ticket.dishes.dishName}</p>
                <div>
                    <button onclick={() => ticket.quantity < 100 ? ticket.quantity++ : ticket.quantity }>+</button>
                    <p>{ticket.quantity}</p>
                    <button onclick={() => ticket.quantity > 0 ? ticket.quantity-- : ticket.quantity }>-</button>
                </div>
            </div>
        {/each}
        <button onclick={onClickOrder}>Beställ</button>
    {:else}
        <p>No menu selected</p>
    {/if}
</div>