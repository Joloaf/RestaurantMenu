<script lang="ts">
    import '../../../app.css';
    import {cacheHandlerActions, type Order} from '$lib/../stores/cacheHandlerService';
    import type Ticket from "../../../stores/cacheHandlerService";
    
    let {currentOrders} = $props<{ currentOrders: Order[] | null}>();
    let orders: Order[] = $state(populateOrders())
    
    function populateOrders(): Order[] {
        
        const activeMenuId = cacheHandlerActions.getActiveCache().currentMenu?.menuId;
        
        if (!activeMenuId) {
            console.warn("No active menu selected");
            return [];
        }
        const ordersArray = currentOrders ?? [];
        
        const currentOrder = [...ordersArray].filter(o => o.menuId === activeMenuId);
        
        return [...currentOrder].sort((a, b) => a.ticketNumber - b.ticketNumber);
    }
    
    function finishTicket(ticketId: number): void {
        cacheHandlerActions.removeTicket(ticketId)

        const orderIndex = orders.findIndex((o: { ticket: Ticket[]; }) =>
            o.ticket.some(t => t.id === ticketId))

        orders[orderIndex].ticket =
            orders[orderIndex].ticket.filter((t: { id: number; }) => t.id !== ticketId);

        if (orders[orderIndex].ticket.length == 0) {
            orders.splice(orderIndex, 1);
        }
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
