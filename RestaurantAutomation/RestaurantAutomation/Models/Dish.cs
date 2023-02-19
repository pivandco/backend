namespace RestaurantAutomation.Models;

using System.ComponentModel.DataAnnotations.Schema;

public sealed class Dish
{
    public int Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    public required string Name { get; set; }

    public string Summary { get; set; } = "";

    public string Description { get; set; } = "";

    public List<DishTag> DishTags { get; set; } = new();

    // TODO: image
}
