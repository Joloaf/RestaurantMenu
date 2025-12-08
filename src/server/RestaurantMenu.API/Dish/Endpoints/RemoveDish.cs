using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class RemoveDish : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapDelete("/{dishId}", Handler);


    public static async Task<IResult> Handler(
        int dishId,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var dish = await context.Dishes.Where(x => x.Id == dishId).SingleOrDefaultAsync();
            
            if (dish == null)
                return TypedResults.NotFound();

            context.Dishes.Remove(dish);
            
            dish.Name = "Empty";
            dish.FoodPicture = "Empty";

            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error removing dish: {e.Message}");
            return TypedResults.InternalServerError();
        }
    }
}