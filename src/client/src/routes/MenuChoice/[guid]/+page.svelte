<script lang="ts">
    import { onDestroy, onMount } from "svelte";
    import { type Menu } from '$lib/services/MenuService'
    import RestMenu from "$lib/components/RestMenu.svelte";
	import { ApiService } from "$lib/services/apiService";
    import type { PageLoad } from "./$types"
    import { page } from '$app/state'
    import { MenuService } from '$lib/services/MenuService'
    import type { ApiResponse } from '$lib/services/MenuService'
	import { goto } from "$app/navigation";
	import { CapacitorCookies } from "@capacitor/core";
	import TicketComponent from "$lib/components/TicketComponent.svelte";
	import OrderView from "$lib/components/OrderView.svelte";

    let { guid } = $props();
    let EditMode = $state(false)
    let locg :string | null = null;
    let menus: Menu[] | null = $state(null)

    let AppPage = $state("MenuChoice");
    let sessiosnStart : Menu[] = []
    let menuDelete : Menu[] = []
    let selectedMenu: Menu | null = null;

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
        console.log(`menus::: ${menus}`)
        menus.forEach((x,y) => sessiosnStart.push({
            menuId: x.menuId,
            userId: x.userId,
            menuName: x.menuName,
            userName: x.userName,
            theme: x.theme,
            dishes: x.dishes
        } as Menu))
        console.log("____________________________________________")
        console.log(menus);
        if(menus.length == 0)
        {
            menus = []
            for(let i = 0; i < 5; i++){
                menus.push({
                    userId : page.params.guid,
                    theme : "0d43ac18-9c65-44f4-b551-85018fe0e007",
                    userName : "Defaultuser",
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
        const toUpdade :Menu[] = menus?.filter((x,y) => sessiosnStart.find((f,z) => f.menuId == x.menuId)) ?? []
        let updateProimses :Promise<Menu>[] = [];
        let addPromises :Promise<Menu>[] = [];
        let deletePromises : Promise<ApiResponse>[] = [];
        let settledDelete = [];
        const toAdd = menus?.filter((x,y) => toUpdade.findIndex((z,f) => z.menuId == x.menuId) == -1) ?? []

        console.log(deletePromises)
        for(let z =0; z < menuDelete.length; z++){
            deletePromises.push(new MenuService(new ApiService()).deleteMenu(menuDelete[z].menuId, menuDelete[z].userId))
            deletePromises = [];
        }
        settledDelete = await Promise.allSettled(deletePromises)
        for(let k = 0; k < deletePromises.length; k++){
            if(settledDelete[k].status == 'rejected')
                console.log(settled[k])
        }
        if(toUpdade.length != 0)
            for(let k = 0; k < toUpdade.length; k++)
            {
                updateProimses.push(new MenuService(new ApiService()).updateMenu(toUpdade[k]))
            }
        let settled = await Promise.allSettled(updateProimses);
        if(toAdd.length != 0)
            for(let z = 0; z < toAdd.length; z++)
        {
            addPromises.push(new MenuService(new ApiService()).createMenu(toAdd[z]))
        }
        settled = await Promise.allSettled(addPromises)
        for(let k = 0; k < settled.length; k++){
            if(settled[k].status == 'rejected')
                console.log(settled[k])
        }
    }
    function onSelectedCB(eventargs :Menu){
        selectedMenu = eventargs;
    }
    function onClickDelete(eventargs :number){
        let d = 0
        console.log(eventargs)
        menus?.filter((x,y) => {
            if(x.menuId == eventargs){
                d = y;
                menuDelete.push(menus![d])
                console.log(menus![d])
            }
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
                    theme : "0d43ac18-9c65-44f4-b551-85018fe0e007",
                    userName : "Defaultuser",
                    menuName : `DefaultMenu${id}`,
                    menuId : id,
                    dishes : [],
                } as Menu)
	}


	async function prev(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        AppPage = AppPage == "MenuChoice" ? "Orderview" : "Orders"
	}


	function next(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        goto(`../../`)
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
{#if AppPage == "MenuChoice"}
<div>
    <p>Menuchoice</p>
    <div >
        <button onclick={onClickEdit}>edit</button>
    </div>
    {#each menus as menu (menu.menuId)}
    <RestMenu 
        menuItem = {menu}
        isEditMode = {EditMode}
        remove={onClickDelete}
        selectedCB = {onSelectedCB}
        children = {null}/>
    {/each}
    {#if EditMode}
    <button onclick={updateDB}>save</button>
    <button onclick={addButton}>+</button>
    {/if}
    {#if !EditMode}
    <button onclick={prev}>Prev</button>
    <button onclick={next}>Next</button>
    {/if}
</div>
{/if}

{#if AppPage == "Orderview"}
    <OrderView currentMenu = {selectedMenu} />
{/if}

{#if AppPage == "Orders"}
{/if}
