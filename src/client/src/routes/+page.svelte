<script lang="ts">
	import { onMount } from 'svelte';
	import { MenuService } from '$lib/services/MenuService'
	import { ApiService  } from '$lib/services/apiService';
	import type { Menu } from '$lib/services/MenuService';
	import { getAllUsersDev } from '$lib/services/devUsersService';


	let menus = {} as Menu

	let loading = true;
	let error = '';
	let userguids :string[] = [];

	onMount(async () => {
		let error = null;
		let men = new MenuService(new ApiService())
		try{

			userguids = await getAllUsersDev();
			console.log(userguids);
			
			if(userguids.length > 0)
				menus = await men.getMenuByMenuId(1)
		}
		catch(exc){
			error = exc;
		}

	});
</script>

<h1>Welcome to Cafe Lek.!!!!</h1>

{#if userguids.length == 0}
	<p>none yet</p>
{/if}
{#each userguids as guid}
	<p>{guid}</p>
{/each}

{#if error != null}
	<p style="color: red">{error}
		{console.log(error)}
	</p>
{/if}
<div>
	<p>{menus.menu_mame}</p>
	<p>{menus.id}</p>
	<p>{menus.theme}:  - {menus.user_name}</p>
</div>
