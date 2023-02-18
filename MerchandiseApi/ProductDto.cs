namespace MerchandiseApi;

using System.Text.Json.Serialization;

public sealed class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }

    [JsonConstructor]
    public ProductDto()
    {
    }

    public ProductDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Price = product.Price;
        CategoryId = product.ProductCategoryId;
    }

    public Product ToProduct() =>
        new()
        {
            Id = Id,
            Name = Name,
            Price = Price,
            ProductCategoryId = CategoryId
        };
}
