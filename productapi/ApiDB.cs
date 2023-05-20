using Microsoft.EntityFrameworkCore;
using task3_attempt2.Models;

namespace task3_attempt2;

public class ApiDB : DbContext
{
    public ApiDB(DbContextOptions<ApiDB> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
}