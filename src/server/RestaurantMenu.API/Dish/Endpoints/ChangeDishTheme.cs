using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Dish.Endpoints;

public class ChangeDishTheme : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapPatch("/theme/{id}", Handler);

    //public record PatchParams(string userid, int menuid);
    //define handler
    private record Request(string File);

    private record Response(string oldTheme);

    private static async Task<IResult> Handler(
        [FromRoute] int id,
        [FromBody] Request request,
        [FromServices] RestaurantDbContext context,
        HttpContext httpContext)
    {
        try
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return TypedResults.Unauthorized();
            }

            if (request.File == null || request.File.Length == 0)
            {
                return TypedResults.NotFound("File is empty");
            }

            var dish = await context.Dishes.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (dish == null)
            {
                return TypedResults.NotFound("Menu not found");
            }

            string oldTheme = dish.FoodPicture;
            dish.FoodPicture = request.File;

            await context.SaveChangesAsync();
            return TypedResults.Ok(new Response(oldTheme));

        }
        catch (Exception err)
        {
            return TypedResults.BadRequest(err.Message);
        }
    }
}