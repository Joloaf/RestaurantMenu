using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RestaurantMenu.Core.Models;

namespace RestaurantMenu.API.Tests.Fixtures;

public static class SeedTestDataBase
{
    public static async Task<string> AddUsers<TProgram>(this WebApplicationFactory<TProgram> host) where TProgram : class
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
}