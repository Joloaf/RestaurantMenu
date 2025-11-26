using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Infrastructure.Data;

public class GetAllMenus : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapGet("/all", Handler);

    public static async Task<Results<Ok<List<MenuModel>>, InternalServerError>> Handler(
        string userId,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var menus = await context.Menus
                .Include(m => m.User)
                .Where(m => m.User.Id == userId)
                .ToListAsync();

            var menusToReturn = menus.Select(menu => new MenuModel(
                menu.Id,
                menu.MenuName,
                menu.UserName,
                menu.Theme,
                menu.User.Id))
                .ToList();

            return TypedResults.Ok(menusToReturn);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving menus: {e.Message}");
            return TypedResults.InternalServerError();
        }

    }
}
