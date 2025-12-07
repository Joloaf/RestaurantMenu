using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;


public class UpdateMenu : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPatch("/{id}", Handler);

    //public record PatchParams(string userid, int menuid);
    //define handler
    public record ValidationErrorModel(MenuDto Dto, string reason);
    
    public static async Task<IResult> Handler(
        [FromRoute] int id,
        [FromBody] MenuDto menuDto,
        [FromServices] RestaurantDbContext context,
        [FromServices] IMenuValidator menuValidator)
    {
        /*
        if(!editModelValidator.EditModelValid(menuModel))
            return TypedResults.BadRequest(new ValidationErrorModel(menuModel, "Not Yet Implemented"));
            */
        
        try
        {
            var user = await context.Users.Where(x => x.Id == menuDto.User_id)
                .Include(x => x.Menus)
                .SingleOrDefaultAsync();
            if(user == null)     // Checking if user is null before trying to access it for item from user.Menus
                return TypedResults.NotFound();
            
            var item = user.Menus.Where(x => x.Id == id).SingleOrDefault();
            if(item == null)
                return TypedResults.NotFound();
            
            item.MenuName = menuDto.Menu_name;
            item.UserName = menuDto.User_name;
            item.Theme = menuDto.Theme;

            // Updates should automatically be tracked, and we should only need the one save.
            //context.Update(item);
            //await context.SaveChangesAsync();
            //context.Update(user);
            await context.SaveChangesAsync();
            
            return TypedResults.Ok<MenuDto>(new MenuDto(item.Id,
                item.MenuName,
                item.UserName,
                item.Theme,
                item.User.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating menu: {e.Message}");
            return TypedResults.InternalServerError();
        }
    }

}