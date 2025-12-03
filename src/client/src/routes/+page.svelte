<script lang="ts">
    import '../app.css';
	import Admin from '$lib/components/menuSwitches/admin.svelte';
	import Everymenu from '$lib/components/menuSwitches/everymenu.svelte';
	import Orders from '$lib/components/menuSwitches/orders.svelte';
	import TicketView from '$lib/components/menuSwitches/ticketView.svelte';
    
	let { data } = $props();
	
	let currentView: 'admin' | 'everymenu' | 'orders' | 'tickets' = $state('everymenu');
</script>


<div class="layout-wrapper">
    <header>
        <img src="/img/logo.png" alt="logo" class="logo" />

        <div class="navigation">
            <button on:click={() => currentView = 'everymenu'} class:active={currentView === 'everymenu'}>Everymenu</button>
            <button on:click={() => currentView = 'orders'} class:active={currentView === 'orders'}>Orders</button>
            <button on:click={() => currentView = 'tickets'} class:active={currentView === 'tickets'}>Tickets</button>
            <button on:click={() => currentView = 'admin'} class:active={currentView === 'admin'}>Admin</button>
        </div>
    </header>

    <main>
        
        <div class="currentView">
            
            
            
        </div>

        
    </main>

    <footer>
        <p>Footer</p>
    </footer>
</div>

{#if currentView === 'admin'}
	<Admin menus={data.menus} currentMenu={data.currentMenu} />
{:else if currentView === 'orders'}
	<Orders currentMenu={data.currentMenu} />
{:else if currentView === 'tickets'}
	<TicketView orders={data.orders} />
{:else if currentView === 'everymenu'}
	<Everymenu menus={data.menus} currentMenu={data.currentMenu} />
{/if}





