using RestaurantMenu.API.Service.DTOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class GetAllDishes : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapGet("/", Handler);

    public static async Task<Results<Ok<List<DishDto>>, InternalServerError>> Handler(
        int menuId,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var dishes = await context.Dishes
                .Where(m => m.Menu.Id == menuId)
                .ToListAsync();

            var dishesToReturn = dishes.Select(dish => new DishDto(
                    dish.Id,
                    dish.Name,
                    dish.FoodPicture))
                .ToList();
            
            return TypedResults.Ok(dishesToReturn);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving menus: {e.Message}");
            return TypedResults.InternalServerError();
        }

    }
}