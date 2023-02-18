namespace MerchandiseApi;

public sealed class ProductCategory
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}
