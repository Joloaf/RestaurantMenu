using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Service;

public class DevelopmentSeedService : IDevelopmentSeedService
{
    private readonly RestaurantDbContext _ctx;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DevelopmentSeedService> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;
    
    public DevelopmentSeedService(RestaurantDbContext context,
        UserManager<User> userManager,
        ILogger<DevelopmentSeedService> logger, 
        IWebHostEnvironment appbuilder,
        IPasswordHasher<User> passwordHasher)
    {
        _ctx = context;
        _userManager = userManager;
        _logger = logger;
        _passwordHasher =  passwordHasher;
        
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
            Email = "Test@Test.com",
            PhoneNumberConfirmed = true,
            PhoneNumber = "+460732019353",
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = "SomeUserName",
        });
        
        if (!use.Succeeded)
            throw new Exception(use.Errors.Select(x=> x.Description).Aggregate((x, y) => x + ", " + y));
        
        var users = _userManager.FindByNameAsync("SomeUserName").GetAwaiter().GetResult();
        await _userManager.AddPasswordAsync(users, "asdasd");
        
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
                UserName = "Someusername",
                Dishes = new List<Core.Models.Dish>()
                {
                    new Core.Models.Dish()
                    {
                        Name = "SeedingDish1",
                        FoodPicture = "taco-8029161_640.png"
                    },
                    new Core.Models.Dish()
                    {
                        Name = "SeedingDish2",
                        FoodPicture = "taco-8029161_640.png"
                    },
                    new Core.Models.Dish()
                    {
                        Name = "SeedingDish3",
                        FoodPicture = "taco-8029161_640.png"
                    }
                }
            });
            await _ctx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}