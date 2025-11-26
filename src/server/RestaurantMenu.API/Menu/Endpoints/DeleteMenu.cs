using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Infrastructure.Data;

public class DeleteMenu : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config) =>
        config.MapDelete("/{id}", Handler);

    public static async Task<Results<NoContent, NotFound, InternalServerError>> Handler(
        [FromRoute] int id,
        [FromQuery] string userId,
        [FromServices] RestaurantDbContext context)
    {
        try
        {
            var menuItem = await context.Menus
                .Where(m => m.Id == id)
                .Include(m => m.User)
                .SingleOrDefaultAsync();
            
            if (menuItem == null)
                return TypedResults.NotFound();

            // Verify the menu belongs to the requesting user
            if (menuItem.User.Id != userId)
                return TypedResults.NotFound();
            
            context.Remove(menuItem);
            await context.SaveChangesAsync();
            
            return TypedResults.NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting menu: {e.Message}");
            return TypedResults.InternalServerError();
        }
    }
}
