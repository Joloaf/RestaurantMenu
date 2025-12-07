using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.Infrastructure.Data;

public class GetMenu : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapGet("/single/{id}", Handler);

    public static async Task<Results<Ok<MenuDto>, NotFound, InternalServerError>> Handler(
        int id,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var menu = await context.Menus
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
                return TypedResults.NotFound();

            return TypedResults.Ok(new MenuDto(
                menu.Id,
                menu.MenuName,
                menu.UserName,
                menu.Theme,
                menu.User.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error retrieving menu: {e.Message}");
            return TypedResults.InternalServerError();
        }

    }
}
