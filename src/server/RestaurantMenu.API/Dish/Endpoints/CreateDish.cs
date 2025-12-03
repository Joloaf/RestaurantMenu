using RestaurantMenu.API.Service.DTOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class CreateDish : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapGet("/", Handler);

    public static async Task<Results<Ok<DishModel>, InternalServerError>> Handler(
        [FromBody] DishModel dish,
        int menuId,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var menu = await context.Menus.FindAsync(menuId);
            
            var newDish = new Core.Models.Dish
            {
                Name = dish.DishName,
                FoodPicture = dish.DishPicture,
                Menu = menu
            };
            
           var dishToReturn=  context.Add(newDish);
           context.SaveChanges();
            
           return TypedResults.Ok(new DishModel(
               newDish.Id,
               newDish.Name,
               newDish.FoodPicture));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating dish: {e.Message}");
            return TypedResults.InternalServerError();
        }

    }
}