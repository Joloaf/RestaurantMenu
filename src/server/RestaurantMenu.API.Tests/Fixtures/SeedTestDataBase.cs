using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.Extensions.DependencyInjection;
using RestaurantMenu.Core.Models;

namespace RestaurantMenu.API.Tests.Fixtures;

public static class SeedTestDataBase
{
    public record SignedInUser(string uid, HttpClient client);
    public static async Task<SignedInUser> CreateSignedInClient<TProgram>(this WebApplicationFactory<TProgram> host) where TProgram : class
    {
        var scope = host.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var pwHasher = scope.ServiceProvider.GetService<PasswordHasher<User>>();
        await userManager.CreateAsync(new User
        {
            UserName = "Bartek",
            Email = "SomeEmail@Somewhere.org",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            PhoneNumber = "+35988888888",
            SecurityStamp = Guid.NewGuid().ToString(),
            
        });
        var us = await userManager.FindByEmailAsync("SomeEmail@Somewhere.org");
        userManager.AddPasswordAsync(us, "hejhej");
        
        return await host.SignedInUserClient(us.Id, "hejhej", "Bartek");
    }

    public static async Task<SignedInUser> SignedInUserClient<TProgram>(this WebApplicationFactory<TProgram> host, string uid, string passw, string uname) where TProgram : class
    {
        var client = host.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true,
            BaseAddress = new Uri("https://localhost:5187"),
            HandleCookies = true,
            MaxAutomaticRedirections = 5
        });
        var res = await client.PostAsJsonAsync("Account/Login", new { username = uname, password = passw });
        
        if(res.IsSuccessStatusCode)
            return new SignedInUser(uid, client);
        
        throw new AccessViolationException("Login failed");
    }

   // public static async Task<HttpClient> CreateSignInUserClient<TProgram>(this WebApplicationFactory<TProgram> host,
   //     string uid) where TProgram : class
   // {
   //     var scope = host.Services.CreateScope();
   //     var siginManaer = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
   //     var manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
   //     var user = await manager.FindByIdAsync(uid);
   //     //is Claimtypes.NameIdentitfier a uid itself or is it id of the current signin session?
   //     
   //     var res = await manager.AddClaimsAsync(user, new Claim[]{ new Claim(ClaimTypes.NameIdentifier, user.) });
   //     if (!res.Succeeded)
   //         throw new AccessViolationException(res.Errors.First().Description);

   //     var httpClient = new HttpClient(new CookieContainerHandler())

   // }
}