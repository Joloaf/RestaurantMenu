using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;


public class MapPatch : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPatch("/", Handler);

    //public record PatchParams(string userid, int menuid);
    //define handler
    public record ValidationErrorModel(MenuModel model, string reason);
    
    public static async Task<IResult> Handler(
        [FromBody] MenuModel menuModel,
        [FromServices] RestaurantDbContex ctx,
        [FromServices] IEditModelValidator editModelValidator,
        [FromServices] IFactory<Menu> factory)
    {
        if(!editModelValidator.EditModelValid(menuModel))
            return TypedResults.BadRequest(new ValidationErrorModel(menuModel, "Not Yet Implemented"));
        
        try
        {
            
            var usr = await ctx.Users.Where(x => x.Id == menuModel.User_id)
                .Include(x => x.Menus)
                .SingleOrDefaultAsync();
            var item = usr.Menus.Where(x => x.Id == menuModel.Id).SingleOrDefault();
            if(usr == null || item == null)
                return TypedResults.NotFound();
            
            item.MenuName = menuModel.Menu_mame;
            item.UserName = menuModel.User_name;
            item.Theme = menuModel.Theme;
            
            ctx.Update(item);
            await ctx.SaveChangesAsync();
            ctx.Update(usr);
            await ctx.SaveChangesAsync();
            
            return TypedResults.Ok<MenuModel>(new MenuModel(item.Id,
                item.MenuName,
                item.UserName,
                item.Theme,
                item.User.Id));
            
        }
        catch (Exception exc)
        {
            return TypedResults.InternalServerError();
        }
    }

}