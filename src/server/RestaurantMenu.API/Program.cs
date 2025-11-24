using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Endpoint.Extensions;
using RestaurantMenu.API.Service;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;
using Scalar.AspNetCore;

namespace RestaurantMenu.API;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        // Configure Database
        builder.Services.AddDbContext<RestaurantDbContex>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configure Identity
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

        builder.Services.AddScoped<IValidations, Validations>();
        builder.Services.AddScoped<IFactory<Menu>, MenuFactory>();
        

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

        app.MapGroup("/Menu")
            .AddMenuFeatures();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.UseCors("AllowAll");
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();


        app.Run();
    }
}