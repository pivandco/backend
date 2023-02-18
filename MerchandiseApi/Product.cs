namespace MerchandiseApi;

public sealed class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    
    public ProductCategory? ProductCategory { get; set; }
    public int? ProductCategoryId { get; set; }
}
