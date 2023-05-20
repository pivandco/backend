namespace RestaurantAutomation.Models;

using System.ComponentModel.DataAnnotations.Schema;

public sealed class Dish
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string Summary { get; set; } = "";

    public string Description { get; set; } = "";

    public List<DishTag> DishTags { get; set; } = new();
}
