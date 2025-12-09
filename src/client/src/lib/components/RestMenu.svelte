<script lang='ts'>
    import { type Menu } from '$lib/services/MenuService'
	import type { Component } from 'svelte';
    import RestDish from './RestDish.svelte';
    import { cacheHandlerActions } from '../../stores/cacheHandlerService';
    import UploadPicture from './uploadPicture.svelte';

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

    function handleMenuPictureUpload(pictureDataUrl: string) {
        theme = pictureDataUrl;
        // The theme is bound to parent, so it will update automatically
        // If you need to save to backend/cache immediately, do it here
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
    <div class="menu-edit-section">
        <div class="image-upload-section">
            <img 
                src={theme?.length > 0 ? (theme.startsWith('data:') ? theme : defPicPath+"/"+theme) : '/pictures/menu-5507525_640.webp'} 
                class="theme-display"
                alt="Menu theme"
            >
            <UploadPicture 
                onPictureSelected={handleMenuPictureUpload}
                buttonText="Change Theme"
            />
        </div>
        <input type="text" bind:value={name} placeholder="Menu name">
    </div>
    {/if}
</div>
        

    <style>
    
    .menu-edit-section {
        display: flex;
        align-items: center;
        gap: 1rem;
        width: 100%;
    }
    
    .image-upload-section {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 0.5rem;
    }
    
    .theme-display {
        width: 150px;
        height: 150px;
        object-fit: cover;
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