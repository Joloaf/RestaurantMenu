using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

public class CreateMenu : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPost("/", Handler);

    public record ValidationErrorModel(MenuModel model, string reason);

    public static async Task<IResult> Handler(
        [FromBody] MenuModel model,
        [FromServices] RestaurantDbContext context,
        [FromServices] IEditModelValidator editModelValidator,
        [FromServices] IFactory<Menu> factory,
        HttpContext httpContext)
    {
        if(!editModelValidator.EditModelValid(model))
            return TypedResults.BadRequest(new ValidationErrorModel(model, "Not Yet Implemented"));
        
        try
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("Error: User not authenticated");
                return TypedResults.Unauthorized();
            }
            
            var user = await context.Users.Where(x => x.Id == userId)
                .Include(x => x.Menus)
                .SingleOrDefaultAsync();
            
            if (user == null)
            {
                Console.WriteLine($"Error: User {userId} not found in database");
                return TypedResults.NotFound();
            }
            Menu menuItem = factory.Create();
            menuItem.MenuName = model.Menu_name;
            menuItem.UserName = model.User_name;
            menuItem.Theme = model.Theme;
            menuItem.User = user;
            menuItem.Dishes = [];

            context.Add(menuItem);
            user.Menus.Add(menuItem);

            // No need to track updates and should therefore only need the one save.
            //await context.SaveChangesAsync();
            //context.Update(menuItem);
            await context.SaveChangesAsync();

            return TypedResults.Created<MenuModel>("",
                new MenuModel(menuItem.Id,
                    menuItem.MenuName,
                    menuItem.UserName,
                    menuItem.Theme,
                    menuItem.User.Id));
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error creating menu: {e.Message}");
            return TypedResults.Problem($"An internal error occurred while retrieving menus: {e.Message}");        }
    }
}
