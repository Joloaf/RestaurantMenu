<script lang='ts'>
    import { type Menu } from '$lib/services/MenuService'
	import type { Component } from 'svelte';
    import RestDish from './RestDish.svelte';
    import { cacheHandlerActions } from '../../stores/cacheHandlerService';

	import { render } from 'svelte/server';
    const defPicPath = "/pictures"
    let clicked = $state(false);

    let { 
        isEditMode, 
        theme = $bindable("default"),
        name = $bindable("default"),
        } = $props()
        
    const boundName : string = $derived(name);
    const boundTheme : string = $derived(theme);

    function onClickImage(){
        
    }
    function OnSelectedMenu(event: MouseEvent & {currentTarget: EventTarget & HTMLButtonElement;}){
        event.stopImmediatePropagation();
        //cacheHandlerActions.setCurrentMenu(menuItem)
    }
    function onClickMenu(){

        if(!clicked){
            clicked = true;
            return;
        }
        if(clicked)
            clicked = false;

    }
   
    function Show(){
        console.log("--------------------------------")
        //console.log(menuItem);
    }
    Show();


	
</script>


    
    
<div class="row">
    {#if !isEditMode}
    <img src={defPicPath+"/"+theme}>
    <!--{@html render(children)}-->
    <p>{name}</p>
    {/if}
    {#if isEditMode}
    <img src={(()=>{
        console.log(theme);
        return theme.length > 0 ? defPicPath+"/"+theme : '/pictures/menu-5507525_640.webp'})()} onclick={onClickImage} class="theme-display">
    <input type="text" bind:value={name}>
    <button type="button" class="pickmeny" onclick={OnSelectedMenu}> Välj meny</button>
    {/if}
</div>
        

    <style>
    
    .theme-display{
        width: 20%;
        height: auto;
        object-fit: contain;
    }
    .pickmeny {
        background: rgb(155, 115, 6);
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
        justify-content: space-evenly;
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
