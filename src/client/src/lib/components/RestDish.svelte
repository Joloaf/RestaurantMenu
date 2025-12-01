<script lang="ts">
import { type Dish } from "$lib/services/DishService";
    let {
        dishes = $bindable([]), 
        active,
        edit,
        children
    } = $props()
    

    function def(){
        if(dishes.length == 0)
        dishes = tempDish;
    }
    let tempDish :Dish[] = [
        {
        name : "hello",
        foodPicture : ""
        } as Dish,
        {   
        name : "hello",
        foodPicture : ""
        } as Dish,
        {   
        name : "hello",
        foodPicture : ""
        } as Dish,
        {   
        name : "hello",
        foodPicture : ""
        } as Dish,
        {   
        name : "hello",
        foodPicture : ""
        } as Dish,
    ]
    function onImageClick(){

    }
    def();


	function onClickDelete(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        event.currentTarget.parentElement?.remove()
	}


	function add(event: MouseEvent & { currentTarget: EventTarget & HTMLButtonElement; }) {
        dishes.push({
            name : "DefaultDish",
            foodPicture : "DefaultPict"
        } as Dish)
	}
</script>
<style>
    .row{
        display: flex;
        flex-direction: row;
    }
</style>
<!--AddEdit-->
<div>
    {#if active}
    {#each dishes as dish}
    {#if !edit}
    <div class='row'>
        <img src={dish.foodPicture} alt="oops.."/>
        {@render children?.()}
        <p>{dish.name}</p>
    </div>
    {/if}
    {#if edit}
    <div>
        <div class='row'>
            <img src={dish.foodPicture} alt="oops.." onclick={onImageClick}>
            <input type="text" bind:value={dish.name}>
            <button onclick={onClickDelete}>-</button>
        </div>
    </div>
    {/if}
    {/each}
    {#if edit}
    <button onclick={add}>+</button>
    {/if}
    {/if}
</div>