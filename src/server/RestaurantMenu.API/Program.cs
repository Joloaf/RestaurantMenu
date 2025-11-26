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
using RestaurantMenu.API.EndPoints.Menu;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.API.Service;


public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<RestaurantDbContex>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();

        // Configure Database
        builder.Services.AddDbContext<RestaurantDbContex>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IValidations, Validations>();
        builder.Services.AddScoped<IEditModelValidator, EditModelValidator>();
        builder.Services.AddScoped<IFactory<Menu>, MenuFactory>();
        builder.Services.AddTransient(typeof(DevelopmentSeedService));


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



       app.MapGroup("/Menu")
            .AddMenuFeatures();


       app.MapApplicationEndPoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            MapGetAllUsersDevelopment
            .MapDevEndPoint(
            app.MapGroup("/Development"),
                  app.Environment);
            
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.UseCors("AllowAll");
            //app.UseCors("*");
            await app.Seed();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        //
        app.Run();
    }
}