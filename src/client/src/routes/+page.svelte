<script lang="ts">
	import { onMount } from 'svelte';
	import { MenuService } from '$lib/services/MenuService';
	import {ApiService} from '$lib/services/apiService.ts';
	
	const ApiServices = new ApiService();
	const menuService = new MenuService(ApiServices);

onMount(async () => {
	const menu = await menuService.getMenusByUserId();
	console.log(menu);
});

</script>

<h1>Welcome to Cafe Lek!!!!</h1>
<a href="/login">Login</a>
<a href="/register">Register</a>

