using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Core.Models;


namespace RestaurantMenu.Infrastructure.Data;

public class RestaurantDbContext : IdentityDbContext<User>
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    
    // dotnet ef database update --startup-project "..\RestaurantMenu.API"

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Menus)
            .WithOne(m => m.User)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Menu>()
            .HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}

