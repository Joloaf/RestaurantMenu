using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class UpdateDish : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPatch("/", Handler);
    
    
    public static async Task<Results<Ok<DishDto>, NotFound, InternalServerError>> Handler(
        [FromBody] DishDto dishDto,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var dish = await context.Dishes.Where(x=> x.Id == dishDto.Id).SingleOrDefaultAsync();
            
            if (dish == null)
                return TypedResults.NotFound();
            
            dish.Name = dishDto.DishName;
            dish.FoodPicture = dishDto.DishPicture;
            
            context.SaveChangesAsync();

            return TypedResults.Ok(new DishDto(
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