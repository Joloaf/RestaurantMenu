namespace RestaurantMenu.API.Service;

public static class SeedingExtensions
{
    public static async Task Seed(this IHost app)
    {
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDevelopmentSeedService>();
        await seeder.SeedAsync();
    }
}