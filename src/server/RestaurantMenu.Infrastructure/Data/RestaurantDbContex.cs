using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.Core.Models;


namespace RestaurantMenu.Infrastructure.Data;

public class RestaurantDbContex : IdentityDbContext<User>
{
    public RestaurantDbContex(DbContextOptions<RestaurantDbContex> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<User>()
            .HasMany<Menu>()
            .WithOne(x => x.User)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        base.OnModelCreating(modelBuilder);
    }
}

