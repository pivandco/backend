using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantAutomation.Models;

namespace RestaurantAutomation;

public sealed class RestaurantAutomationDbContext : IdentityDbContext<IdentityUser>
{
    public RestaurantAutomationDbContext(DbContextOptions<RestaurantAutomationDbContext> options) : base(options)
    {
    }

    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishTag> DishTags => Set<DishTag>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            UserName = "Admin",
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Admin123!")
        });
    }
}