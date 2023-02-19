namespace RestaurantAutomation.Models;

public sealed class DishTag
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public List<Dish> Dishes { get; set; } = new();
}
