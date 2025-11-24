using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using MinimalApi.Endpoint;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantMenu.Core.Models;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Endpoint.Extensions;
using Microsoft.AspNetCore.Http.Connections.Features;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Database
builder.Services.AddDbContext<RestaurantDbContex>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IValidateMenu, ValidateMenu>();


// Add CORS for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

//I realized AFTER the fact that im mapping a get and then inserting a model value :)
//I can't think, code and speak at the same time :)
//


app.MapEndpoints();

app.MapGet("/Menu", async 
);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseCors("AllowAll");
}
<<<<<<< HEAD

//app.UserRouting();

=======
>>>>>>> main
app.UseHttpsRedirection();


app.Run();

public static class MenuFeatureExtension
{
    public static RouteGroupBuilder AddMenuFeatures(this RouteGroupBuilder group)
    {
        group.MapGet("/", AddHandler);
        group.MapPost("/", CreateHandler);
        group.MapDelete("/", DeleteHandler);
        group.MapPatch("/", );
    }
    public record AddMenuModel(string UserName, int UserId);

    public static async Task<IResult> EditHandler([FromBody] int id,
                                                  [FromServices] RestaurantDbContex dbContext,
                                                  [FromServices] HttpContext httpcontext)
    {

    }

    public static async Task<IResult> DeleteHandler([FromBody] int id,
                                                    [FromServices] RestaurantDbContex dbcontext,
                                                    [FromServices] HttpContext httpcontext)
    {

    }
    public static async Task<IResult> CreateHandler([FromBody] AddMenuModel addMenuModel, 
                                              [FromServices] RestaurantDbContex context,
                                              [FromServices] HttpContext httpContext)
    {

    }
    public static async Task<IResult> AddHandler([FromBody] MenuModel model,
                                    [FromServices] RestaurantDbContex context,
                                    [FromServices] HttpContext provider)
    {
        var validator  = provider.RequestServices.GetRequiredService<IValidateMenu>();
        var dbModelFactory = provider.RequestServices.GetRequiredService<IFactory<Menu>>();
        Menu modelItem = dbModelFactory.Create();

        if (validator.ValidateMenu(model))
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

public interface IValidateMenu
{
    public bool ValidateMenu(string menuName);
    public bool ValidateMenuName(string name);
}

public class MenuFactory : IFactory<Menu>
{
    public Menu Create()
    {
        //object creation.

        //Mutate and insert default values here.

        //insert default values here.
        return new Menu();
    }
}

public class ValidateMenu : IValidateMenu
{
    bool IValidateMenu.ValidateMenu(MenuModel model)
    {
        return true;
    }
}

public record MenuModel(int id, string name, string userName, string? theme, int userId);


public interface IFactory<T>
{
    T Create();
}


public class GetMenuEndPoint()
{

}