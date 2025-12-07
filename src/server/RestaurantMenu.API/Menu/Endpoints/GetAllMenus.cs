using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

public class GetAllMenus : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapGet("/all", Handler);

    
    record MenuDishesDTO(string menuId, string menuName, string theme, string userName, ICollection<DishDto> dishes);


    public static async Task<IResult> Handler(
        [FromServices] RestaurantDbContext context,
        HttpContext httpContext)
    {
        try
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return TypedResults.Unauthorized();
            }

            var menus = await context.Menus
                .Include(m => m.User)
                .Include(u => u.Dishes)
                .Where(m => m.User.Id == userId)
                .ToListAsync();


            return TypedResults.Ok<List<MenuDishesDTO>>(menus.Select((x) => 
                new MenuDishesDTO(x.Id.ToString(),
                    x.MenuName,
                    x.Theme,
                    x.UserName,
                    x.Dishes.Select(y=> new DishDto(y.Id, y.Name, y.FoodPicture))
                        .ToList()))
                .ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving menus: {e.Message}");
            return TypedResults.InternalServerError();
        }

    }
}
