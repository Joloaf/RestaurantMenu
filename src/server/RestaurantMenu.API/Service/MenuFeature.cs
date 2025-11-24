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
                                                    [FromServices] HttpContext httpcontext)
    {

        throw new NotImplementedException();
    }
    public static async Task<IResult> GetHandler([FromBody] AddMenuModel addMenuModel, 
                                              [FromServices] RestaurantDbContex context,
                                              [FromServices] HttpContext httpContext)
    {

        throw new NotImplementedException();
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
        modelItem.Id = null;
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