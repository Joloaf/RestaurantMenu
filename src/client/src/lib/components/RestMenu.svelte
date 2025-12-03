<script lang='ts'>
    import { type Menu } from '$lib/services/MenuService'
	import type { Component } from 'svelte';
    import RestDish from './RestDish.svelte';
    import { cacheHandlerActions } from '../../stores/cacheHandlerService'
	import { render } from 'svelte/server';
    let clicked = $state(false);

    let { 
        menuItem: initialMenuItem,
        isEditMode,
        remove,
        selectedCB,
        children
    } = $props()

    let menuItem = $state(initialMenuItem);

    function onClickImage(){
        
    }
    function OnSelectedMenu(event: MouseEvent & {currentTarget: EventTarget & HTMLButtonElement;}){
        event.stopImmediatePropagation();
        cacheHandlerActions.setCurrentMenu(menuItem)
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
        event.stopImmediatePropagation();
        cacheHandlerActions.removeMenu(menuItem);
	}
</script>

<div class="row" onclick={onClickMenu} >
    {#if !isEditMode}
    <img src="{menuItem.theme}">
    <!--{@html render(children)}-->
    <p>{menuItem.menuName}</p>
    {/if}
    {#if isEditMode}
        <img src={menuItem.theme > 0 ? menuItem.theme : '/pictures/menu-5507525_640.webp'} onclick={onClickImage}>
        <input type="text" bind:value={menuItem.menuName}>
        <button  type="button" class="remove" onclick={onClickDelete}>-</button>
        <button type="button" class="pickmeny" onclick={OnSelectedMenu}> Välj meny</button>
    {/if}
</div>
{#if clicked}
<div class="column">
    <RestDish 
    dishes   = {menuItem.dishes}
    active   = {clicked}
    edit     = {isEditMode}
    menuId   = {menuItem.menuId}
    children = {children}/>
</div>
{/if}

    <style>
    .pickmeny {
        background: rgb(229, 252, 26);
        color: rgb(238, 238, 238);
        border: none;
        padding: 0.5rem 1rem;
        cursor: pointer;
        border-radius: 4px;
    }
    .remove {
        background: #ff4444;
        color: white;
        border: none;
        padding: 0.5rem 1rem;
        cursor: pointer;
        border-radius: 4px;
    }
    .row{
        display: flex;
        flex-direction: row;
        gap: 1.125rem;
        border: 1px solid black;
        border-radius: 2px;
        padding: 0.5rem;
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
