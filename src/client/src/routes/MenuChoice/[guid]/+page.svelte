<script lang="ts">
    import { onDestroy, onMount } from "svelte";
    import { type Menu } from '$lib/services/MenuService'
    import { MenuService } from '$lib/services/MenuService'
    import RestMenu from "$lib/components/RestMenu.svelte";
	import { ApiService } from "$lib/services/apiService";
    import type { PageLoad } from "./$types"
    import { page } from '$app/state'

    let { guid } = $props();
    let EditMode = $state(false)
    let locg :string | null = null;
    let menus: Menu[] | null = $state(null)
    export const load: PageLoad = async ({ params }) =>{
        console.log(guid)
        locg = params.guid;
        menus = await (new MenuService(new ApiService())).getMenusByUserId(page.params.guid)
        console.log(menus);
        if(menus == null || menus.length == 0)
        {
            menus = []
            for(let i = 0; i < 5; i++){
                menus.push({
                    userId : params.guid,
                    theme : "someTheme",
                    userName : "DefaultUser",
                    menuName : `DefaultMenu${i}`,
                    menuId : i,
                } as Menu)
            }
        }
    }
    onMount(async ()=>{
        menus = await (new MenuService(new ApiService())).getMenusByUserId(page.params.guid)
        console.log(menus);
        if(menus.length == 0)
        {
            menus = []
            for(let i = 0; i < 5; i++){
                menus.push({
                    userId : page.params.guid,
                    theme : "someTheme",
                    userName : "DefaultUser",
                    menuName : `DefaultMenu${i}`,
                    menuId : i,
                    dishes : [],
                } as Menu)
            }
        }
        console.log(menus)
    })

   // onMount(async () =>{
   //     console.log(guid)
   //     menus = await (new MenuService(new ApiService())).getMenusByUserId(guid)
   // })
 function onClickEdit(){
     if(!EditMode)
     {
         EditMode = true;
         return;
        }
        EditMode = false;
    }
    async function updateDB(){
        
    }
    function onClickDelete(eventargs){
        let d = 0
        menus?.filter((x,y) => {
            if(x.menuId == eventargs)
                d = y;
        })
        menus?.splice(d, 1);
    }


	function addButton(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        if(menus?.length != 0)
            var id = menus![menus!.length-1].menuId+1;
        else
            var id = 0;
        menus?.push({
                    userId : page.params.guid,
                    theme : "someTheme",
                    userName : "DefaultUser",
                    menuName : `DefaultMenu${id}`,
                    menuId : id,
                    dishes : [],
                } as Menu)
	}
</script>
<style>
    .row{
        display: flex;
        flex-direction: row;
    }
    .editbutton{
        display: flex;
        flex-direction: row;
        flex-flow: row-reverse;
    }
    </style>
<p>Menuchoice</p>
<div >
    <button onclick={onClickEdit}>edit</button>
</div>
{#each menus as menu (menu.menuId)}
<RestMenu menuItem = {menu} isEditMode = {EditMode} remove={onClickDelete}/>
{/each}
{#if EditMode}
<button onclick={updateDB}>save</button>
<button onclick={addButton}>+</button>
{/if}
