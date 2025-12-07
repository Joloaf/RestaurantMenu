using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class RemoveDish : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapDelete("/{menuId}", Handler);


    public static async Task<Results<Ok<DishDto>, NotFound, InternalServerError>> Handler(
        int id,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var dish = await context.Dishes.FindAsync(id);

            if (dish == null)
                return TypedResults.NotFound();

            dish.Name = "Empty";
            dish.FoodPicture = "Empty";

            await context.SaveChangesAsync();

            return TypedResults.Ok(new DishDto(
                dish.Id,
                dish.Name,
                dish.FoodPicture));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error removing dish: {e.Message}");
            return TypedResults.InternalServerError();
        }
    }
}