<script lang="ts">
	import { onMount } from "svelte";
    import { page } from '$app/state'
    import { MenuService } from '$lib/services/MenuService'
    import { ApiService } from "$lib/services/apiService";
    import type { Menu } from "$lib/services/MenuService"
	import RestMenu from "$lib/components/RestMenu.svelte";
    import { CapacitorCookies } from "@capacitor/core";
	import type { Dish } from "$lib/services/DishService";
	import RestDish from "./RestDish.svelte";
    
    let { menu } = $props()

    onMount(async ()=>{ 
        
    })

</script>

<div>

</div>