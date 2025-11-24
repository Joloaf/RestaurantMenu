using Microsoft.AspNetCore.Mvc;

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
    public record AddMenuModel(string UserName, int UserId);

    public static async Task<IResult> EditHandler([FromBody] int id,
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
    public static async Task<IResult> AddHandler([FromBody] MenuModel model,
                                    [FromServices] RestaurantDbContex context,
                                    [FromServices] HttpContext provider)
    {
        var validator  = provider.RequestServices.GetRequiredService<IValidations>();
        var dbModelFactory = provider.RequestServices.GetRequiredService<IFactory<Menu>>();
        Menu modelItem = dbModelFactory.Create();

        if (validator.ValidMenuName(model.name))
        {
            modelItem.MenuName = model.name;
            modelItem.UserName = model.userName;
            modelItem.Id = model.id;
            modelItem.Theme = model.theme;

            context.Add(modelItem);
            try
            {
                await context.SaveChangesAsync();

            }
            catch(Exception _)
            {
                return Results.InternalServerError();
            }

            return Results.Accepted();
        }
        return Results.BadRequest();
    }
}