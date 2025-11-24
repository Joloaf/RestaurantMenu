using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantMenu.Core.Models;
using Microsoft.AspNetCore.Identity;
namespace RestaurantMenu.API.Tests.Fixtures
{
    public class WebclassFixture<TProgram> : WebApplicationFactory<TProgram>  where TProgram : class
    {
        public WebclassFixture()
        {
        }
        public string UserGuid { get; set; }
        public async Task<string> AddUsers(WebApplicationFactory<TProgram> host)
        {
            var scope = host.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            userManager.CreateAsync(new User
            {
                UserName = "Bartek",
                Email = "SomeEmail@Somewhere.org",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "+35988888888",
                SecurityStamp = Guid.NewGuid().ToString()
            });
            var us = await userManager.FindByEmailAsync("SomeEmail@Somewhere.org");
            return us.Id.ToString();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Step 1: remove real services from our web application to avoid manipulation of real database during testing
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(RestaurantMenu.Infrastructure.Data.RestaurantDbContex));
                if (descriptor != null)
                    services.Remove(descriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(SqliteConnection));
                if (dbConnectionDescriptor != null)
                {
                    services.Remove(dbConnectionDescriptor);
                }

                //Step 2: set up services that the test server will use in memory
                services.AddSingleton<SqliteConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });

                services.AddDbContext<RestaurantMenu.Infrastructure.Data.RestaurantDbContex>((container, options) =>
                {
                    var connection = container.GetRequiredService<SqliteConnection>();
                    object value = options.UseSqlite(connection);
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RestaurantMenu.Infrastructure.Data.RestaurantDbContex>();
            db.Database.EnsureCreated();
            
            return host;
        }
    }
}
