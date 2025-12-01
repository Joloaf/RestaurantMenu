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
using RestaurantMenu.API.Service.Validations;


public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //TODO change requires after dev
        builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6; // dev = asdasd
            })
            .AddEntityFrameworkStores<RestaurantDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();

        // Configure Database
        builder.Services.AddDbContext<RestaurantDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IValidations, Validations>();
        builder.Services.AddScoped<IEditModelValidator, EditModelValidator>();
        builder.Services.AddScoped<IFactory<Menu>, MenuFactory>();
        builder.Services.AddTransient<IDevelopmentSeedService, DevelopmentSeedService>();


        // Add CORS for development
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("http://localhost:5173", "http://192.168.0.190:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
                
                policy.WithOrigins("http://localhost:5173/*", "http://192.168.0.190:5173/*")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials(); 
                
                policy.WithOrigins("http://localhost:5173/MenuChoice/*", "http://192.168.0.190:5173/MenuChoice/*")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();

              //// policy.AllowAnyOrigin()
              //     .AllowAnyMethod()
              //     .AllowAnyHeader();

            });
        });

        var app = builder.Build();

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