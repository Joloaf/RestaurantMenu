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
        group.MapDelete("/", DeleteHandler);
        group.MapPatch("/", EditHandler);
        group.MapGet("/", GetHandler);
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
    public static async Task<Results<Ok<MenuModel>, NotFound>> EditHandler([FromBody] MenuModel menuModel,
                                                  RestaurantDbContex dbContext,
                                                  HttpContext httpcontext)
    {
        return TypedResults.Ok(new MenuModel(1, "some", "else", "theme", "use_id"));
    }
    

    public static async Task<IResult> DeleteHandler([FromBody] MenuModel model,
                                                    [FromServices] RestaurantDbContex dbcontext,
                                                    HttpContext httpcontext)
    {
        return Results.Ok();
    }
    public static async Task<IResult> GetHandler(int id, 
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