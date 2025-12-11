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
    
    function getMenuName(menuId: string): string {
        const menu = cacheHandlerActions.getMenu(menuId);
        return menu?.menuName || `Meny #${menuId}`;
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

<div class="tickets-container">
    <h2 class="title">🎫 Beställningar ({orders.length})</h2>
    
    {#if orders.length > 0}
        <div class="tickets-list">
            {#each orders as order}
                {#if order.ticket.length != 0}
                    <div class="ticket-card">
                        <div class="ticket-header">
                            <span class="ticket-number">Ticket #{order.ticketNumber}</span>
                            <span class="menu-name">{getMenuName(order.menuId)}</span>
                        </div>
                        
                        {#if order.ticket && order.ticket.length > 0}
                            <div class="ticket-items">
                                <div class="items-header">Rätter ({order.ticket.length})</div>
                                <div class="dishes-list">
                                    {#each order.ticket as item}
                                        {#if item.quantity > 0}
                                            <div class="dish-item">
                                                <div class="dish-info">
                                                    <span class="dish-name">{item.dishes.dishName || `Rätt #${item.dishes.id}`}</span>
                                                    <span class="dish-quantity">× {item.quantity}</span>
                                                </div>
                                                <button 
                                                    class="btn-finish" 
                                                    onclick={() => finishTicket(item.id)}
                                                > ✓ Färdig </button>
                                            </div>
                                        {/if}
                                    {/each}
                                </div>
                            </div>
                        {:else}
                            <p class="no-items">Inga rätter i denna beställning</p>
                        {/if}
                    </div>
                {/if}
            {/each}
        </div>
    {:else}
        <p class="no-tickets">Inga beställningar tillgängliga</p>
    {/if}
</div>

<style>
    .tickets-container {
        padding: 1rem;
    }
    
    .title {
        color: #FF6B6B;
        font-size: 2.2rem;
        font-weight: bold;
        margin-bottom: 1.5rem;
        text-align: center;
    }
    
    .tickets-list {
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
    }
    
    .ticket-card {
        background: white;
        border-radius: 16px;
        padding: 1.5rem;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
    }
    
    .ticket-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
    }
    
    .ticket-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1.5rem;
        padding-bottom: 1rem;
        border-bottom: 3px solid #FFB347;
    }
    
    .ticket-number {
        font-size: 1.5rem;
        font-weight: bold;
        color: #FF6B6B;
    }
    
    .menu-name {
        font-size: 1.1rem;
        color: #666;
        font-weight: 600;
    }
    
    .ticket-items {
        margin-top: 1rem;
    }
    
    .items-header {
        font-size: 1.3rem;
        font-weight: bold;
        color: #333;
        margin-bottom: 1rem;
    }
    
    .dishes-list {
        display: flex;
        flex-direction: column;
        gap: 0.75rem;
    }
    
    .dish-item {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 1rem;
        background: linear-gradient(135deg, #FFF9E6 0%, #FFE5F1 100%);
        border-radius: 12px;
        border: 2px solid #FFB347;
        transition: all 0.2s ease;
    }
    
    .dish-item:hover {
        transform: translateX(4px);
        box-shadow: 0 2px 8px rgba(255, 179, 71, 0.3);
    }
    
    .dish-info {
        display: flex;
        flex-direction: column;
        gap: 0.25rem;
        flex: 1;
    }
    
    .dish-name {
        font-size: 1.2rem;
        font-weight: bold;
        color: #333;
    }
    
    .dish-quantity {
        font-size: 1.1rem;
        color: #FF6B6B;
        font-weight: 600;
    }
    
    .btn-finish {
        background: linear-gradient(135deg, #a8ff78 0%, #00CED1 100%);
        color: white;
        border: 2px solid #00A8AA;
        padding: 0.75rem 1.5rem;
        cursor: pointer;
        border-radius: 12px;
        font-size: 1.2rem;
        font-weight: bold;
        box-shadow: 0 4px 8px rgba(0, 206, 209, 0.4);
        transition: all 0.2s ease;
        text-shadow: 1px 1px 2px #00A8AA, -1px -1px 2px #00A8AA, 1px -1px 2px #00A8AA, -1px 1px 2px #00A8AA;
    }
    
    .btn-finish:hover {
        transform: scale(1.1);
        box-shadow: 0 6px 12px rgba(0, 206, 209, 0.5);
    }
    
    .btn-finish:active {
        transform: scale(0.95);
    }
    
    .no-items {
        text-align: center;
        color: #888;
        font-size: 1.2rem;
        padding: 1rem;
        font-style: italic;
    }
    
    .no-tickets {
        text-align: center;
        color: #888;
        font-size: 1.5rem;
        margin-top: 2rem;
    }
</style>
