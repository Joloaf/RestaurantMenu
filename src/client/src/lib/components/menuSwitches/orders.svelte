<script lang="ts">
    import '../../../app.css';
    import RestMenu from "../RestMenu.svelte";
    import type { Menu } from '$lib/services/MenuService';
    import type Ticket from "../../../stores/cacheHandlerService";
    import { type Order} from "../../../stores/cacheHandlerService"
    import { cacheHandlerActions } from "../../../stores/cacheHandlerService";

    let { currentMenu } = $props<{ currentMenu: Menu | null }>();
    let orders : Order = $state(populateOrder())
    
    function populateOrder() : Order{
        const orders :Order = { } as Order;
        orders.ticket = currentMenu.dishes.map(x  => {
            return ({dishes: x, id: Math.floor(Math.random() * 1000), quantity: 0} as Ticket);
        })
        orders.menuId = currentMenu.menuId;
        orders.ticketNumber = Math.floor(Math.random() * 1000); 

        return orders;
    }
    function onClickOrder(order: Order) : void {
        order.ticket = order.ticket.filter(x => x.quantity !== 0)
        console.log(order)
        cacheHandlerActions.addOrder(order);
        orders = populateOrder()
    }
</script>

<div class="orders-container">
    {#if currentMenu}
        <RestMenu 
            theme = {currentMenu.theme}
            name= {currentMenu.menuName}
            isEditMode={false}
        />
        
        <div class="dishes-list">
            {#each orders.ticket as ticket}
                <div class="dish-card">
                    <div class="dish-image-container">
                        <img 
                            src={ticket.dishes?.dishPicture.length ?? -1 > 0 ? "/pictures/"+ticket.dishes.dishPicture : "/pictures/a70a6112-964d-4f87-8853-0ad44b6d4a3a.png"}
                            alt={ticket.dishes.dishName}
                            class="dish-image"
                        />
                    </div>
                    <div class="dish-info-container">
                        <div class="dish-name-section">
                            <h3 class="dish-name">{ticket.dishes.dishName}</h3>
                        </div>
                        <div class="order-controls">
                            <button 
                                class="btn-minus" 
                                onclick={() => ticket.quantity > 0 ? ticket.quantity-- : ticket.quantity}
                            > − </button>
                            
                            <div class="quantity-display">
                                {ticket.quantity}
                            </div>
                            
                            <button 
                                class="btn-plus" 
                                onclick={() => ticket.quantity < 100 ? ticket.quantity++ : ticket.quantity}
                            > + </button>
                        </div>
                    </div>
                </div>
            {/each}
        </div>
        
        <button class="place-order-btn" onclick={() => onClickOrder(orders)}>Beställ</button>
    {:else}
        <p class="no-menu-msg">Ingen meny vald</p>
    {/if}
</div>

<style>
    .orders-container {
        padding: 1rem;
    }
    
    .dishes-list {
        display: flex;
        flex-direction: column;
        gap: 1rem;
        margin-top: 1.5rem;
    }

    .dish-card {
        display: flex;
        flex-direction: row;
        background: white;
        border-radius: 16px;
        overflow: hidden;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        height: 150px;
    }

    .dish-image-container {
        flex: 1;
        min-width: 0;
    }
    
    .dish-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .dish-info-container {
        flex: 1;
        display: flex;
        flex-direction: column;
        min-width: 0;
    }

    .dish-name-section {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 0.5rem;
    }
    
    .dish-name {
        font-size: 1.3rem;
        font-weight: bold;
        color: #333;
        margin: 0;
        text-align: center;
        word-break: break-word;
    }

    .order-controls {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: space-around;
        padding: 0.5rem;
        gap: 0.5rem;
    }

    .btn-minus {
        width: 60px;
        height: 60px;
        background: linear-gradient(135deg, #FC6675 0%, #AB537C 100%);
        color: white;
        border: 2px solid #8B2F5C;
        border-radius: 12px;
        font-size: 2.5rem;
        font-weight: bold;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 4px 8px rgba(255, 107, 157, 0.4);
        transition: all 0.2s ease;
        line-height: 1;
        text-shadow: 1px 1px 2px #8B2F5C, -1px -1px 2px #8B2F5C, 1px -1px 2px #8B2F5C, -1px 1px 2px #8B2F5C;
    }
    
    .btn-minus:hover {
        transform: scale(1.1);
        box-shadow: 0 6px 12px rgba(255, 107, 157, 0.5);
    }
    
    .btn-minus:active {
        transform: scale(0.95);
    }

    .btn-plus {
        width: 60px;
        height: 60px;
        background: linear-gradient(135deg, #a8ff78 0%, #00CED1 100%);
        color: white;
        border: 2px solid #00A8AA;
        border-radius: 12px;
        font-size: 2.5rem;
        font-weight: bold;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 4px 8px rgba(0, 206, 209, 0.4);
        transition: all 0.2s ease;
        line-height: 1;
        text-shadow: 1px 1px 2px #00A8AA, -1px -1px 2px #00A8AA, 1px -1px 2px #00A8AA, -1px 1px 2px #00A8AA;
    }
    
    .btn-plus:hover {
        transform: scale(1.1);
        box-shadow: 0 6px 12px rgba(0, 206, 209, 0.5);
    }
    
    .btn-plus:active {
        transform: scale(0.95);
    }

    .quantity-display {
        width: 70px;
        height: 60px;
        background: white;
        border: 3px solid #FFB347;
        border-radius: 12px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 2rem;
        font-weight: bold;
        color: #333;
        box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .place-order-btn {
        width: 100%;
        background: linear-gradient(135deg, #FFB347 0%, #FF6B6B 100%);
        color: white;
        border: 2px solid #D94A4A;
        padding: 1.5rem 2rem;
        cursor: pointer;
        border-radius: 16px;
        font-size: 1.8rem;
        font-weight: bold;
        margin-top: 2rem;
        text-transform: uppercase;
        box-shadow: 0 6px 12px rgba(255, 107, 107, 0.4);
        transition: all 0.3s ease;
        text-shadow: 1px 1px 2px #D94A4A, -1px -1px 2px #D94A4A, 1px -1px 2px #D94A4A, -1px 1px 2px #D94A4A;
    }
    
    .place-order-btn:hover {
        transform: scale(1.05);
        box-shadow: 0 8px 16px rgba(255, 107, 107, 0.5);
    }
    
    .place-order-btn:active {
        transform: scale(0.98);
    }
    
    .no-menu-msg {
        text-align: center;
        color: #888;
        font-size: 1.5rem;
        margin-top: 3rem;
    }
</style>