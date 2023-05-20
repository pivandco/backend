namespace task3_attempt2.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }

    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
}