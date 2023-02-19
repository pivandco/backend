namespace RestaurantAutomation;

using Microsoft.EntityFrameworkCore;
using Models;

public sealed class RestaurantAutomationDbContext : DbContext
{
    public RestaurantAutomationDbContext(DbContextOptions<RestaurantAutomationDbContext> options) : base(options)
    {
    }

    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishTag> DishTags => Set<DishTag>();
}
