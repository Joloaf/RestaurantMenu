<script lang="ts">
	import { onMount } from "svelte";
    import { page } from '$app/state'
    import { MenuService } from '$lib/services/MenuService'
    import { ApiService } from "$lib/services/apiService";
    import type { Menu } from "$lib/services/MenuService"
	import RestMenu from "$lib/components/RestMenu.svelte";
    import { CapacitorCookies } from "@capacitor/core";

    let menu :Menu;
onMount(async ()=>{ 

    var cookie = JSON.parse(document.cookie);
    console.log(cookie)
    var co = new CookieStore();
    co.get({
        url : 'localhost',
        key : 'MenuTicket'
    } as CookieStoreGetOptions)
    .then((x)=>{
        console.log("cookie found")
        cookie = x
    }, (x) => {
        console.log(`cookie excavation rejected::::${JSON.stringify(x)}`)
    })
    .catch((x)=>{
        console.log(`excavation threw::: ${x}`)
    })
    console.log(document.cookie)
    console.log('*********************************************************************')
    console.log(cookie)

    if(cookie == null)
        menu = await (new MenuService(new ApiService())).getMenuByMenuId(page.params.menuId)
})

</script>

<div>
    <RestMenu 
        selectedCB = {null}
        menuItem = {menu}
        isEditMode = {false}
        remove = {null}
        children = {`<input type="number" min="0" max="100" step='1'/>`}
    />
    
</div>