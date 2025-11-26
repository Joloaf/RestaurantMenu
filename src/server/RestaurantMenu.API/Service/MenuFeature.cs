using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

public static class MenuFeatureExtension
{
    public static RouteGroupBuilder AddMenuFeatures(this RouteGroupBuilder group)
    {
        group.MapPost("/", AddHandler);
        group.MapDelete("/{id}", DeleteHandler);
        group.MapPatch("/", EditHandler);
        group.MapGet("/single", GetSingleHandler);
        group.MapGet("/all", GetAllHandler);
        return group;
    }
    
    /// <summary>
    /// Bartek
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dbContext"></param>
    /// <param name="httpcontext"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static async Task<Results<Ok<MenuModel>, NotFound, BadRequest<ValidationErrorModel>, InternalServerError>> EditHandler([FromBody] MenuModel menuModel,
                                                  RestaurantDbContex ctx,
                                                  IEditModelValidator editModelValidator,
                                                  IFactory<Menu> menuFactory)
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

    public record ValidationErrorModel(MenuModel model, string reason);

    public static async Task<Results<NoContent, NotFound, InternalServerError>> DeleteHandler(
                        [FromRoute] int id,
                        [FromQuery] string userId,
                        RestaurantDbContex dbContext,
                        HttpContext httpContext)
    {
        var menuItem = await dbContext.Menus
            .Where(m => m.Id == id)
            .Include(m => m.User)
            .SingleOrDefaultAsync();
        
        if (menuItem == null)
            return TypedResults.NotFound();

        if (menuItem.User.Id != userId)
            return TypedResults.NotFound();
        
        dbContext.Remove(menuItem);
        
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (Exception exc)
        {
            return TypedResults.InternalServerError();
        }
        
        return TypedResults.NoContent();
    }
    public static async Task<IResult> GetSingleHandler(int id, 
                                              [FromServices] RestaurantDbContex context,
                                                HttpContext httpContext)
    {
        var menu = await context.Menus
            .Include(m => m.User) 
            .FirstOrDefaultAsync(m => m.Id == id);
        
        return menu != null ? Results.Ok(new MenuModel(menu.Id,
            menu.MenuName,
            menu.UserName,
            menu.Theme,
            menu.User.Id)) : Results.NotFound();
    }
    
    public static async Task<IResult> GetAllHandler(string userId, 
        [FromServices] RestaurantDbContex context,
        HttpContext httpContext)
    {
        var menus = await context.Menus
            .Include(m => m.User)
            .Where(m => m.User.Id == userId)
            .ToListAsync();

        List<MenuModel> menusToReturn = new List<MenuModel>();
        foreach (var menu in menus)
        {
            var toAdd = new MenuModel(menu.Id,
                menu.MenuName,
                menu.UserName,
                menu.Theme,
                menu.User.Id);
            menusToReturn.Add(toAdd);
        }

        return menusToReturn != null ? Results.Ok(menusToReturn) : Results.NotFound();
    }
    public static async Task<Results<Ok<MenuModel>, NotFound, InternalServerError>> AddHandler([FromBody] MenuModel model,
                                    RestaurantDbContex context,
                                    HttpContext provider)
    {
    //    var validator  = provider.RequestServices.GetRequiredService<IValidations>();
        var dbModelFactory = provider.RequestServices.GetRequiredService<IFactory<Menu>>();
        Menu modelItem = dbModelFactory.Create();

        var user =  await context.Users.Where(x => x.Id == model.User_id)
            .Include(x=> x.Menus)
            .SingleOrDefaultAsync();
    
       if(user == null) 
           return TypedResults.NotFound();

        
        modelItem.MenuName = model.Menu_mame;
        modelItem.UserName = model.User_name;
        modelItem.Theme = model.Theme;
        modelItem.User = user;
        modelItem.Dishes = [];
        
        context.Add(modelItem);
        user.Menus.Add(modelItem);

        try
        {
            
            await context.SaveChangesAsync();
            context.Update(modelItem);
            await context.SaveChangesAsync();
        }
        catch(Exception exc)
        {
            return TypedResults.InternalServerError();
        }
        return TypedResults.Ok<MenuModel>(
            new MenuModel(modelItem.Id,
            modelItem.MenuName,
            modelItem.UserName,
            modelItem.Theme,
            modelItem.User.Id)
            );

       // var createdModel = new MenuModel(modelItem.Id, modelItem.MenuName, modelItem.UserName, modelItem.Theme, model.user_id);
       // return TypedResults.Ok<MenuModel>(createdModel);
    }
}