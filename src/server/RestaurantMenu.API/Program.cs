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
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.API.Service;


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

        builder.Services.AddScoped<IValidations, Validations>();


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

        app.UseHttpsRedirection();


        app.Run();
    }
}