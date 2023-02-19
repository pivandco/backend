namespace RestaurantAutomation.Dto;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class DishRequestDto
{
    public int Id { get; set; }
    [Required] public required string Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public List<int> TagIds { get; set; } = new();

    public Dish ToDish(IQueryable<DishTag> tags) =>
        new()
        {
            Id = Id,
            Name = Name,
            Summary = Summary ?? "",
            Description = Description ?? "",
            DishTags = tags.Where(t => TagIds.Contains(t.Id)).ToList()  // TODO: handle missing tags
        };
}
