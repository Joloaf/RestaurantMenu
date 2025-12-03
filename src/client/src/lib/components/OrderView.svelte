<script lang="ts">
import { type Dish } from "$lib/services/DishService";
import {type Menu } from "$lib/services/MenuService"
export class OrderdDish{
    
}
export class DishOrder{
    constructor(dish :Dish, dishAmount :number){
        this.selectedDish = dish;
        this.amount = dishAmount;
    }
    public selectedDish :Dish;
    public amount : number;

    
}
export const {
    currentMenu = $bindable(), 
    orderedDishes = $bindable([]),
} = $props();

function def(){
        if(currentMenu.dishes.length == 0 || currentMenu.dishes == null || currentMenu.dishes == undefined)
            currentMenu.dishes = tempDish;
        
            currentMenu.dishes.forEach((x,y) =>{ orderedDishes.push(new DishOrder(x, 0))});
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
    def();
</script>
<style>
    .row{
        display: flex;
        flex-direction: row;
    }
</style>
<!--AddEdit-->
<div>
    <div class="row">
        <img src="{currentMenu.theme}"/>
        <p>{currentMenu.menuName}</p>
    </div>
    {#each orderedDishes as dish}
    <div class='row'>
        <img src={dish.selectedDish.foodPicture} alt="oops.."/>
        <input type="number" step="1" min="0" max="100" bind:value={dish.amount}>
        <p>{dish.selectedDish.name}</p>
    </div>
    {/each}
    <button on:click={() =>{/*Handle Order Event,
         the orderedDishes prop is loaded*/}}>Order</button>
</div>