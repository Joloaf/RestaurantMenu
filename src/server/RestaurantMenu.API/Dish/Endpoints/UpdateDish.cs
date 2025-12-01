using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class UpdateDish : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPatch("/", Handler);
    
    
    public static async Task<Results<Ok<DishModel>, NotFound, InternalServerError>> Handler(
        [FromBody] DishModel dishModel,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var dish = await context.Dishes.FindAsync(dishModel.Id);
            
            if (dish == null)
                return TypedResults.NotFound();
            
            dish.Name = dishModel.DishName;
            dish.FoodPicture = dishModel.DishPicture;

            return TypedResults.Ok(new DishModel(
                dish.Id,
                dish.Name,
                dish.FoodPicture));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving menu: {e.Message}");
            return TypedResults.InternalServerError();
        }

    }
}