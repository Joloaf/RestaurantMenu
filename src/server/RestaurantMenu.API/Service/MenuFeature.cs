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
        group.MapGet("/", AddHandler);
        group.MapPost("/", CreateHandler);
        group.MapDelete("/", DeleteHandler);
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
                                                  [FromServices] HttpContext httpcontext)
    {
        throw new NotImplementedException();
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
                                    [FromServices] HttpContext provider)
    {
        var validator  = provider.RequestServices.GetRequiredService<IValidations>();
        var dbModelFactory = provider.RequestServices.GetRequiredService<IFactory<Menu>>();
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

        return TypedResults.Ok<MenuModel>(model);
    }
}