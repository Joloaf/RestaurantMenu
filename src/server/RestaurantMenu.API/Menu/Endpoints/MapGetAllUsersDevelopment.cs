using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.EndPoints.Menu;

public class MapGetAllUsersDevelopment : IDevelopmentEndPoint
{
    public static void MapDevEndPoint(IEndpointRouteBuilder config, IWebHostEnvironment env)
    {
       if (!env.IsDevelopment())
            throw new Exception("No peeking users while in prod");

       config.MapGroup("/Users")
           .MapGet("/GetAll", Handler);
    }

    private static async Task<IResult> Handler(
        [FromServices] RestaurantDbContext ctx,
        [FromServices] IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            // I CANT THROW AND RETURN A NOTFOUND AT THE SAME TIME 
            // BUT IF COULD I WOULD
            return TypedResults.NotFound();
        }
        return TypedResults.Ok<List<string>>(await ctx.Users.Select(x=> x.Id).ToListAsync());
    }
}
