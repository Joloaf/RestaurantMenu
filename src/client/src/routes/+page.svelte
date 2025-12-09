<script lang="ts">
    import '../app.css';
	import Admin from '$lib/components/menuSwitches/admin.svelte';
	import Everymenu from '$lib/components/menuSwitches/everymenu.svelte';
	import Orders from '$lib/components/menuSwitches/orders.svelte';
	import TicketView from '$lib/components/menuSwitches/ticketView.svelte';
	import { cacheHandlerActions } from '../stores/cacheHandlerService';
	import type { Menu } from '$lib/services/MenuService.js';
	import { MenuService } from '$lib/services/MenuService.js';
	import { DishService } from '$lib/services/DishService.js';
	import { ApiService } from '$lib/services/apiService.js';

	const apiService = new ApiService();
	const menuService = new MenuService(apiService);
	const dishService = new DishService(apiService);
	//where is data defined/populated?
	let { data } = $props();
	
	let currentView: 'admin' | 'everymenu' | 'orders' | 'tickets' = $state('everymenu');
	//
	//child handler for the swap event from admin page.
	//capture method and data to update correctly
	//
	let childHandler: ()=>Promise<void>;

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
	//the methods run once as soon as the page gets loaded... im not sure about the context.
	//pass a method to the child to provide the method and the context for whenever we swap pages
	//
	function assignSwap(handlePageSwap : () => Promise<void>){
		childHandler = handlePageSwap;
	}
	//call into the child context, with the handler from the child to update as soon 
	//as the page gets swapped
	async function signalSwap(){
		//make sure cache is up to date
		await childHandler?.();

		//push to db, the children deal with creation and deletion in the db
		//
		// (granted they should'nt, but we'd have to alter the cache model
		//  to something that would allow us to track what happened to the given object
		//  something like 
		// 		const alter = { command: string = "update" | "delete" | "create"; }
		// then the children just interact with the cache, we handle db updates after.)
		//
		await PushDB(cacheHandlerActions.getActiveCache().menus);
	}
	async function PushDB(menus :Menu[]){
		const pushMenus = async () => {
			for(let i = 0; i < menus.length; i++)
				await menuService.updateMenu(menus[i])
		};
		const pushDishes = async ()=>{
			const dishes =[ ...menus.flatMap((x) => x.dishes)]
			for(let i =0; i < dishes.length; i++){
				console.log(dishes[i]);
				await dishService.upDateDish(dishes[i])

			}
		}
		await pushMenus();
		await pushDishes();
	}
</script>


<div class="layout-wrapper">
    <header>
        <img src="/img/logo.png" alt="logo" class="logo" />

        <a href="/login">Login</a>
        <a href="/register">Register</a>

        <div class="navigation">
            <button onclick={async () => { await signalSwap(); currentView = 'everymenu'; /*GetCurrentCacheData()*/}} class:active={currentView === 'everymenu'}>Everymenu</button>
            <button onclick={async () => { await signalSwap(); currentView = 'orders';    /*GetCurrentCacheData()*/}} class:active={currentView === 'orders'}>Orders</button>
            <button onclick={async () => { await signalSwap(); currentView = 'tickets';   /*GetCurrentCacheData()*/}} class:active={currentView === 'tickets'}>Tickets</button>
            <button onclick={() => {  currentView = 'admin';     /*GetCurrentCacheData()*/}} class:active={currentView === 'admin'}>Admin</button>
        </div>
    </header>

    <main>
        
        <div class="currentView">
			{#if currentView === 'admin'}
				<Admin 
					menus={data.menus} 
					currentMenu={data.currentMenu}
					assignHandler = {assignSwap}
					/>
			{:else if currentView === 'orders'}
				<Orders currentMenu={cacheHandlerActions.getActiveCache().currentMenu} />
			{:else if currentView === 'tickets'}
				<TicketView currentOrders={cacheHandlerActions.getActiveCache().currentOrder} />
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






