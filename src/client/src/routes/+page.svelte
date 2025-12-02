<script lang="ts">
	import Admin from '$lib/components/menuSwitches/admin.svelte';
	import Everymenu from '$lib/components/menuSwitches/everymenu.svelte';
	import Orders from '$lib/components/menuSwitches/orders.svelte';
	import TicketView from '$lib/components/menuSwitches/ticketView.svelte';
	
	let { data } = $props();
	
	let currentView: 'admin' | 'everymenu' | 'orders' | 'tickets' = $state('everymenu');
</script>


<h1>Welcome to Cafe Lek!!!!</h1>
<a href="/login">Login</a>
<a href="/register">Register</a> <!-- This could be moved into layout?? or in admin only? -->

<div class="navigation">
	<button onclick={() => currentView = 'admin'} class:active={currentView === 'admin'}>Admin</button>
	<button onclick={() => currentView = 'everymenu'} class:active={currentView === 'everymenu'}>Everymenu</button>
	<button onclick={() => currentView = 'orders'} class:active={currentView === 'orders'}>Orders</button>
	<button onclick={() => currentView = 'tickets'} class:active={currentView === 'tickets'}>Tickets</button>
</div>
<div>
<div>{currentView}</div>

{#if currentView === 'admin'}
	<Admin menus={data.menus} currentMenu={data.currentMenu} />
{:else if currentView === 'orders'}
	<Orders currentMenu={data.currentMenu} />
{:else if currentView === 'tickets'}
	<TicketView orders={data.orders} />
{:else if currentView === 'everymenu'}
	<Everymenu menus={data.menus} currentMenu={data.currentMenu} />
{/if}




</div>