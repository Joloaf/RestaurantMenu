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

    public static async Task<Results<Created<MenuModel>, NotFound, BadRequest<ValidationErrorModel>, InternalServerError>> Handler(
        [FromBody] MenuModel model,
        [FromServices] RestaurantDbContext context,
        [FromServices] IEditModelValidator editModelValidator,
        [FromServices] IFactory<Menu> factory)
    {
        if(!editModelValidator.EditModelValid(model))
            return TypedResults.BadRequest(new ValidationErrorModel(model, "Not Yet Implemented"));

        try
        {
            var user = await context.Users.Where(x => x.Id == model.User_id)
                .Include(x => x.Menus)
                .SingleOrDefaultAsync();

            if(user == null)
                return TypedResults.NotFound();

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

            return TypedResults.Created($"/Menu/{menuItem.Id}", new MenuModel(
                menuItem.Id,
                menuItem.MenuName,
                menuItem.UserName,
                menuItem.Theme,
                menuItem.User.Id));
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error creating menu: {e.Message}");
            return TypedResults.InternalServerError();
        }
    }
}
