<script lang="ts">
    import type { Order } from '$lib/../stores/cacheHandlerService';

    let { orders } = $props<{ orders: Order[] }>();
    
    // Get sorted orders by ticket number
    let sortedOrders = $derived(
        orders ? [...orders].sort((a, b) => a.ticketNumber - b.ticketNumber) : []
    );
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
    {#if sortedOrders.length > 0}
        {#each sortedOrders as order}
            <div class="ticket-item">
                <div class="ticket-header">
                    Ticket #{order.ticketNumber} - Menu ID: {order.menuId}
                </div>
                {#if order.ticket && order.ticket.length > 0}
                    <div>
                        <strong>Items ({order.ticket.length}):</strong>
                        {#each order.ticket as item}
                            <div class="dish-item">
                                • Dish ID: {item.dishes.id} × {item.quantity}
                            </div>
                        {/each}
                    </div>
                {:else}
                    <p>No items in this ticket</p>
                {/if}
            </div>
        {/each}
    {:else}
        <p>No tickets available</p>
    {/if}
</div>
