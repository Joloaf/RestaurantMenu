<script lang="ts">
    import '../../../app.css';
    import {cacheHandlerActions, type Order} from '$lib/../stores/cacheHandlerService';
    import type Ticket from '$lib/../stores/cacheHandlerService';


    let {currentOrders} = $props<{ currentOrders: Order[] }>();
    let orders: Order[] = $state(populateOrders())

    function populateOrders(): Order[] {
        return [...currentOrders].sort((a, b) => a.ticketNumber - b.ticketNumber);
    }


    // Get sorted orders by ticket number
    // let sortedOrders = $derived(
    //     currentOrders ? [...currentOrders].sort((a, b) => a.ticketNumber - b.ticketNumber) : []
    // );

    function finishTicket(ticketId: number): void {
        cacheHandlerActions.removeTicket(ticketId)
    }


</script>

<style>
    .ticket-item {
        border: 1px solid #ddd;
        padding: 1rem;
        margin-bottom: 1rem;
        border-radius: 4px;
        background: #f9f9f9;
    }

    .ticket-header {
        font-weight: bold;
        margin-bottom: 0.5rem;
    }

    .dish-item {
        padding: 0.25rem 0;
        margin-left: 1rem;
    }
</style>

<div>
    <h2>All Tickets</h2>
    {#if orders.length > 0}
        {#each orders as order}
            {#if order.ticket.length != 0}
                <div class="ticket-item image-container-menu">
                    <div class="ticket-header">
                        Ticket #{order.ticketNumber} - Menu ID: {order.menuId}
                    </div>
                    {#if order.ticket && order.ticket.length > 0}
                        <div>
                            <strong>Items ({order.ticket.length}):</strong>
                            {#each order.ticket as item}
                                {#if item.quantity > 0}
                                    <div class="dish-item">
                                        • Dish ID: {item.dishes.id} × {item.quantity}
                                        <button onclick={() => finishTicket(item.id)}>Färdig</button>
                                    </div>
                                {/if}
                            {/each}
                        </div>
                    {:else}
                        <p>No items in this ticket</p>
                    {/if}
                </div>
            {/if}
        {/each}

    {:else}
        <p>No tickets available</p>
    {/if}
</div>
