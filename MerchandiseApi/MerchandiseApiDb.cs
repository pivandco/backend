using Microsoft.EntityFrameworkCore;

namespace MerchandiseApi;

public sealed class MerchandiseApiDb : DbContext
{
    public MerchandiseApiDb(DbContextOptions<MerchandiseApiDb> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}