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
app.MapGet("/Menu", async 
([FromBody] MenuModel model,
 [FromServices] RestaurantDbContex context,
 [FromServices] IServiceProvider provider) =>
{
    var validator  = provider.GetRequiredService<IValidateMenu>();
    var dbModelFactory = provider.GetRequiredService<IFactory<Menu>>();
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

});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

app.Run();



public interface IValidateMenu
{
    public bool ValidateMenu(MenuModel model);
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

