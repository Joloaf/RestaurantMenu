<script lang="ts">
	import { onMount, tick } from 'svelte';
	import { MenuService } from '$lib/services/MenuService';
	import {ApiService} from '$lib/services/apiService.ts';
	import type { MemoryCache, Ticket, Order } from '../../stores/cacheHandlerService.ts';
	import Admin from '$lib/compunents/menuSwitches/admin.svelte';
	import Everymenu from '$lib/compunents/menuSwitches/everymenu.svelte';
	import Orders from '$lib/compunents/menuSwitches/orders.svelte';
	import TicketView from '$lib/compunents/menuSwitches/ticketView.svelte';
	
	const ApiServices = new ApiService();
	const menuService = new MenuService(ApiServices);
	
	let menu: any = null
	let currentView: 'admin' | 'everymenu' | 'orders' | 'tickets' = 'everymenu';
	

onMount(async () => {
	const menu = await menuService.getMenusByUserId();
	console.log(menu);
});



</script>

<h1>Welcome to Cafe Lek!!!!</h1>
<a href="/login">Login</a>
<a href="/register">Register</a>

<div class="navigation">
	<button on:click={() => currentView = 'admin'} class:active={currentView === 'admin'}>Admin</button>
	<button on:click={() => currentView = 'everymenu'} class:active={currentView === 'everymenu'}>Everymenu</button>
	<button on:click={() => currentView = 'orders'} class:active={currentView === 'orders'}>Orders</button>
	<button on:click={() => currentView = 'tickets'} class:active={currentView === 'tickets'}>Tickets</button>
</div>
<div>
<div>{currentView}</div>

{#if currentView === 'admin'}
<Admin>

</Admin>
{:else if currentView === 'orders'}
<Orders>

</Orders>
{:else if currentView === 'tickets'}
<TicketView>
	
</TicketView>
{:else if currentView === 'everymenu'}
<Everymenu>

</Everymenu>
{/if}




</div>