using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Service;

public class DevelopmentSeedService
{
    private readonly RestaurantDbContex _ctx;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DevelopmentSeedService> _logger;
    
    public DevelopmentSeedService(RestaurantDbContex context,
        UserManager<User> userManager,
        ILogger<DevelopmentSeedService> logger, 
        IWebHostEnvironment appbuilder)
    {
        _ctx = context;
        _userManager = userManager;
        _logger = logger;
        
        if(!appbuilder.IsDevelopment())
            throw new Exception("Development environment not set, do you want to kill the db?");
    }

    public async Task SeedAsync()
    {
        if (await _ctx.Users.AnyAsync())
        {
           _logger.LogDebug("Users already in Database, (not gonna seed)");
            return;
        }
        
        _ctx.Database.EnsureCreated();
       var use =  await _userManager.CreateAsync(new User()
        {
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            PhoneNumber = "+460732019353",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "SomeUserName",
        });
        
        if (!use.Succeeded)
            throw new Exception(use.Errors.Select(x=> x.Description).Aggregate((x, y) => x + ", " + y));

        var user = await _ctx.Users
            .Include(x=> x.Menus)
            .ThenInclude(x=> x.Dishes)
            .FirstOrDefaultAsync();
        
        if(user == null)
            throw new Exception("User not found");

        try
        {
            user.Menus.Add(new Menu()
            {
                MenuName = "Seedingmenu",
                User = user,
                Theme = Guid.NewGuid().ToString(),
                UserName = "Someusername"
            });
            await _ctx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}