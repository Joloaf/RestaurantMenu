<script lang='ts'>
    import { type Menu } from '$lib/services/MenuService'
    import RestDish from './RestDish.svelte';

    let clicked = $state(false);
    let { 
        menuItem,
        isEditMode,
        remove,
        selectedCB,
        children
    } = $props()

    function onClickImage(){
        
    }
    function onClickMenu(){

        selectedCB(menuItem);
        if(!clicked){
            clicked = true;
            return;
        }
        if(clicked)
            clicked = false;
    }
   
    function Show(){
        console.log(menuItem);
    }
    Show();


	function onClickDelete(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        remove(menuItem.menuId)
	}
</script>
<style>
    .row{
        display: flex;
        flex-direction: row;
        gap: 1.125rem;
    }
    .highlight{
        box-shadow: 27px 21px 97px 39px rgba(28,174,49,0.63);
        -webkit-box-shadow: 27px 21px 97px 39px rgba(28,174,49,0.63);
        -moz-box-shadow: 27px 21px 97px 39px rgba(28,174,49,0.63);
    }
    .highlight-img{
        outline-color: chocolate;
    }
    .column{
        display: flex;
        flex-direction: column;
    }
</style>

<div class="row" onclick={onClickMenu} >
    {#if !isEditMode}
    <img src="{menuItem.theme}">
    <p>{menuItem.menuName}</p>
    {/if}
    {#if isEditMode}
        <img src="{menuItem.theme}" onclick={onClickImage}>
        <input type="text" bind:value={menuItem.menuName}>
        <button onclick={onClickDelete}>-</button>
    {/if}
</div>
{#if clicked}
<div class="column">
    <RestDish 
        dishes   = {menuItem.dishes}
        active   = {clicked}
        edit     = {isEditMode}
        children = {children}/>
</div>
{/if}
