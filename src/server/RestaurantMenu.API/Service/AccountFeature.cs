using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantMenu.Core.Models;

namespace RestaurantMenu.API.Service;

public static class AccountFeatureExtension
{
    public static RouteGroupBuilder AddAccountFeatures(this RouteGroupBuilder group)
    {
        group.MapPost("/register", RegisterHandler);
        group.MapPost("/login", LoginHandler);
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
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return TypedResults.BadRequest(new ErrorResponse(errors));
        }

        return TypedResults.Ok(new UserResponse(user.Id, user.UserName!, user.Email!));
    }

    public static async Task<Results<Ok<UserResponse>, BadRequest<ErrorResponse>>> LoginHandler(
        [FromBody] LoginRequest request,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        
        if (user == null)
        {
            return TypedResults.BadRequest(new ErrorResponse("Invalid username or password"));
        }

        var result = await signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(new ErrorResponse("Invalid username or password"));
        }

        return TypedResults.Ok(new UserResponse(user.Id, user.UserName!, user.Email!));
    }

}
