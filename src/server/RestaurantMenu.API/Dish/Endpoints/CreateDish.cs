using RestaurantMenu.API.Service.DTOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class CreateDish : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPost("/{menuId}", Handler);

    public static async Task<IResult> Handler(
        [FromRoute] string menuId,
        [FromBody] DishModel dish,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            int outInt = -1;
            if(!int.TryParse(menuId, out outInt) || outInt < 1)
                return Results.BadRequest();

            var menu = await context.Menus.Where(x => x.Id == outInt).SingleOrDefaultAsync();
            
            if (menu == null)
                return TypedResults.NotFound("notFound");
            
            
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