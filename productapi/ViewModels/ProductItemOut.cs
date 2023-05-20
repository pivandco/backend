using task3_attempt2.Models;

namespace task3_attempt2.ViewModels;

public class ProductItemOut
{
    public ProductItemOut()
    {
    }

    public ProductItemOut(Product productItem)
    {
        (Id, Name, Price, CategoryId) =
            (productItem.Id, productItem.Name, productItem.Price, productItem.CategoryId);
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
}