namespace MerchandiseApi;

using System.Text.Json.Serialization;

public sealed class ProductCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    [JsonConstructor]
    public ProductCategoryDto()
    {
    }

    public ProductCategoryDto(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }

    public ProductCategory ToProductCategory() => new()
    {
        Id = Id,
        Name = Name
    };
}
