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
    public static async Task<IResult> EditHandler([FromBody] EditMenuModel menuModel,
                                                  [FromServices] RestaurantDbContex dbContext,
                                                  HttpContext httpContext)
    {
        var menu = await dbContext.Menus
            .Where(m => m.Id == menuModel.id)
            .SingleOrDefaultAsync();
            
        if (menu == null)
            return TypedResults.NotFound();
            
        menu.MenuName = menuModel.menu_name ?? menu.MenuName;
        menu.UserName = menuModel.user_account_name ?? menu.UserName;
        menu.Theme = menuModel.theme ?? menu.Theme;
        
        await dbContext.SaveChangesAsync();
        
        var updatedModel = new MenuModel(menu.Id, menu.MenuName, menu.UserName, menu.Theme, menuModel.user_id);
        return TypedResults.Ok(updatedModel);
    }
    

    public static async Task<IResult> DeleteHandler([FromRoute] int id,
                                                    [FromServices] RestaurantDbContex dbcontext,
                                                    [FromServices] HttpContext httpcontext)
    {

        throw new NotImplementedException();
    }
    public static async Task<IResult> CreateHandler([FromBody] AddMenuModel addMenuModel, 
                                              [FromServices] RestaurantDbContex context,
                                              [FromServices] HttpContext httpContext)
    {

        throw new NotImplementedException();
    }
    public static async Task<Results<Ok<MenuModel>, NotFound, InternalServerError>>
        AddHandler([FromBody] MenuModel model,
                                    [FromServices] RestaurantDbContex context,
                                    HttpContext httpContext,
                                    [FromServices] IValidations validator,
                                    [FromServices] IFactory<Menu> dbModelFactory)
    {
        Menu modelItem = dbModelFactory.Create();

        var user =  await context.Users.Where(x => x.Id == model.user_id)
            .Include(x=> x.Menus)
            .SingleOrDefaultAsync();
    
       if(user == null) 
           return TypedResults.NotFound();
       
        modelItem.MenuName = model.menu_mame;
        modelItem.UserName = model.user_name;
        modelItem.Theme = model.theme;
        modelItem.Dishes = [];
        
        user.Menus.Add(modelItem);
        
        try
        {
            await context.SaveChangesAsync();

        }
        catch(Exception _)
        {
            return TypedResults.InternalServerError();
        }

        var createdModel = new MenuModel(modelItem.Id, modelItem.MenuName, modelItem.UserName, modelItem.Theme, model.user_id);
        return TypedResults.Ok<MenuModel>(createdModel);
    }
}