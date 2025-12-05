<script lang="ts">
    import '../app.css';
	import Admin from '$lib/components/menuSwitches/admin.svelte';
	import Everymenu from '$lib/components/menuSwitches/everymenu.svelte';
	import Orders from '$lib/components/menuSwitches/orders.svelte';
	import TicketView from '$lib/components/menuSwitches/ticketView.svelte';
	import { cacheHandlerActions } from '../stores/cacheHandlerService';
    

	//where is data defined/populated?
	let { data } = $props();
	
	let currentView: 'admin' | 'everymenu' | 'orders' | 'tickets' = $state('everymenu');

	function GetCurrentCacheData() {
		let cache = cacheHandlerActions.getActiveCache();

		console.log('Getting current cache data:', cache);
		console.log('Current data before update:', data);


	
		data.menus =  cache.menus,
		data.currentMenu = cache.currentMenu,
		data.orders = cache.orders,
		data.isLoading = cache.isLoading
		console.log('Data after update:', data);
	}
</script>


<div class="layout-wrapper">
    <header>
        <img src="/img/logo.png" alt="logo" class="logo" />

        <div class="navigation">
            <button onclick={() => { currentView = 'everymenu';GetCurrentCacheData()}} class:active={currentView === 'everymenu'}>Everymenu</button>
            <button onclick={() => (currentView = 'orders', GetCurrentCacheData)} class:active={currentView === 'orders'}>Orders</button>
            <button onclick={() => (currentView = 'tickets', GetCurrentCacheData)} class:active={currentView === 'tickets'}>Tickets</button>
            <button onclick={() => (currentView = 'admin', GetCurrentCacheData)} class:active={currentView === 'admin'}>Admin</button>
        </div>
    </header>

    <main>
        
        <div class="currentView">
			{#if currentView === 'admin'}
				<Admin menus={(()=>{ 
					console.log(data.menus)
					return data.menus})()} 
					currentMenu={(()=>{ 
						console.log(data.currentMenu)
						return data.currentMenu})()
						} />
			{:else if currentView === 'orders'}
				<Orders currentMenu={data.currentMenu} />
			{:else if currentView === 'tickets'}
				<TicketView orders={data.orders} />
			{:else if currentView === 'everymenu'}
				<Everymenu menus={data.menus} currentMenu={data.currentMenu} />
			{/if}
            
            
        </div>

        
    </main>

    <footer>
        <p>Footer</p>
    </footer>
</div>
<style>
	.currentView{
		display: block;
		
	}
	main{
		display: block;
		max-height: 50rem;
		overflow-y: scroll;
	}
	
	header{
		display: inline-block;
	}
	footer{
		display: block;
	}
</style>






