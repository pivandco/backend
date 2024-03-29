namespace RestaurantAutomation.Dto;

using Models;

public sealed class DishResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public List<DishTagDto> Tags { get; set; } = new();

    public static DishResponseDto FromDish(Dish dish) =>
        new()
        {
            Id = dish.Id,
            Name = dish.Name,
            Summary = dish.Summary,
            Description = dish.Description,
            Tags = dish.DishTags.Select(t => DishTagDto.FromDishTag(t)).ToList(),
        };
}
