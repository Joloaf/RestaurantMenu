using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RestaurantMenu.API.Service;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantMenu.API.Tests.Fixtures;

public static partial class DBSeederTestConfig
{
    /// <summary>
    /// Declawing the seeder, it attempts to run during testing, for some reason it throws
    /// when internally tyring to add users, odd but yeeting it should solve the issue
    ///
    /// this one is separate from the seeding service in testing enviroment,
    ///     it's INTERNAL to the application itself.
    /// </summary>
    /// <param name="builder"></param>
    internal static void DeclawSeeder(this IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var seederDescriptor = services.SingleOrDefault(x=>
                x.ServiceType == (typeof(DevelopmentSeedService)));
                
            if (seederDescriptor != null)
                services.Remove(seederDescriptor);

            services.AddTransient<IDevelopmentSeedService, TestingSeederService>();
        });
    }
}