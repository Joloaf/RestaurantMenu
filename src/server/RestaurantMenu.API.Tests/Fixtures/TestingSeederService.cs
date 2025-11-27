using RestaurantMenu.API.Service;

namespace RestaurantMenu.API.Tests.Fixtures;

public static partial class DBSeederTestConfig
{
    internal class TestingSeederService : IDevelopmentSeedService
    {

        public Task SeedAsync()
        {
            return Task.CompletedTask;
        }
    }
}