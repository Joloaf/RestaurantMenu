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

    // Empty function, will be implemented at a later stage...
    function onClickImage(){
        
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

<div class="menu-display" class:edit-mode={isEditMode}>
    {#if !isEditMode}
        <img src={defPicPath+"/"+theme} alt={name} class="menu-image">
        <p class="menu-name">{name}</p>
    {/if}
    {#if isEditMode}
        <div class="row">
            <img src={(()=>{
                console.log(theme);
                return theme.length > 0 ? defPicPath+"/"+theme : '/pictures/menu-5507525_640.webp'})()} onclick={onClickImage} class="theme-display">
            <input type="text" bind:value={name}>
        </div>
    {/if}
</div>
        
<style>
    .menu-display {
        display: flex;
        flex-direction: column;
        gap: 0;
    }
    
    .menu-name {
        font-size: 2rem;
        font-weight: bold;
        color: #333;
        margin: 0;
        text-align: center;
    }
    
    .menu-image {
        width: 100%;
        height: 300px;
        object-fit: cover;
        border-radius: 20px;
        border: 5px solid #FFB347;
        box-shadow: 
            0 0 0 3px white,
            0 0 0 6px #FF6B6B,
            0 8px 20px rgba(0, 0, 0, 0.4),
            0 4px 10px rgba(0, 0, 0, 0.2);
        transform: rotate(-1.5deg);
        transition: transform 0.3s ease;
        margin-bottom: 1rem;
    }
    
    .menu-image:hover {
        transform: rotate(0deg) scale(1.02);
    }

    .theme-display{
        width: 20%;
        height: auto;
        object-fit: contain;
    }

    .row{
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: center;
        gap: 1.5rem;
        padding: 1rem;
    }
</style>
