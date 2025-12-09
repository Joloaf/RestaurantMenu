<script lang="ts">
    import '../../../app.css';
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import type { Ticket } from "../../../stores/cacheHandlerService"
    import { type Order} from "../../../stores/cacheHandlerService"
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { currentMenu } = $props<{ currentMenu: Menu | null }>();
    //bind to tickets
    let i = -1;
    let orders : Order = $state(populateOrder())
    

    function populateOrder() : Order{
        const orders :Order = { } as Order;
        orders.ticket = currentMenu.dishes.map(x => ({ dishes: x, id : i++, quantity: 0 } as Ticket))
        orders.menuId = currentMenu.menuId;
        orders.ticketNumber = Math.floor(Math.random() * 100); // Need to change this to a sherd variabel, just random now so it works

        return orders;
    }
    function onClickOrder(order: Order) : void {
        order.ticket = order.ticket.filter(x => x.quantity !== 0)
        console.log(order)
        cacheHandlerActions.addOrder(order);
        orders = populateOrder()
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
                <img class="order-dish-logo" src={ticket.dishes?.dishPicture.length ?? -1 > 0 ? "/pictures/"+ticket.dishes.dishPicture : "/pictures/a70a6112-964d-4f87-8853-0ad44b6d4a3a.png"}/>
                <p>{ticket.dishes.dishName}</p>
                <div>
                    <button onclick={() => ticket.quantity < 100 ? ticket.quantity++ : ticket.quantity }>+</button>
                    <p>{ticket.quantity}</p>
                    <button onclick={() => ticket.quantity > 0 ? ticket.quantity-- : ticket.quantity }>-</button>
                </div>
            </div>
        {/each}
        <button onclick={() => onClickOrder(orders)}>Beställ</button>
    {:else}
        <p>No menu selected</p>
    {/if}
</div>