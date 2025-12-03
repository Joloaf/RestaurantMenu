using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantMenu.Core.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RestaurantMenu.API.Features;

public static class AccountFeatureExtension
{
    public static RouteGroupBuilder AddAccountFeatures(this RouteGroupBuilder group)
    {
        group.MapPost("/register", RegisterHandler);
        group.MapPost("/Login", LoginHandler);
        return group;
    }

    public record RegisterRequest(string Username, string Email, string Password);

    public record LoginRequest(string Username, string Password);

    public record UserResponse(string Id, string Username, string Email);

    public record ErrorResponse(string Message);

    public static async Task<Results<Ok<UserResponse>, BadRequest<ErrorResponse>>> RegisterHandler(
        [FromBody] RegisterRequest request,
        UserManager<User> userManager)
    {
        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
        };

        var menu = new Menu
        {
            MenuName = "Your first menu",
            UserName = request.Username,
            Theme = "Defult"
        };

        for (int i = 0; i < 6; i++)
        {
            menu.Dishes.Add(new Core.Models.Dish
            {
                Name = "Empty",
                FoodPicture = "Empty"
            });
        }
        user.Menus.Add(menu);
        
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return TypedResults.BadRequest(new ErrorResponse(errors));
        }

        return TypedResults.Ok((new UserResponse(user.Id, user.UserName!, user.Email!)));
    }

    public static async Task<Results<Ok<UserResponse>, BadRequest<ErrorResponse>>> LoginHandler(
        [FromBody] LoginRequest request,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<UserResponse> logger)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null)
            return TypedResults.BadRequest(new ErrorResponse("Invalid username or password"));
        var result = await signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(new ErrorResponse("Invalid username or password"));
        }
        Console.WriteLine(user.Id);
        logger.LogInformation(user.Id.ToString());
        return TypedResults.Ok(new UserResponse(user.Id.ToString(), user.UserName!,  user.Email!));
    }
}