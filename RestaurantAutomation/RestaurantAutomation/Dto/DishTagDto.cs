namespace RestaurantAutomation.Dto;

using System.ComponentModel.DataAnnotations;
using Models;

public sealed class DishTagDto
{
    public int Id { get; set; }
    [Required] public required string Title { get; set; }

    public static DishTagDto FromDishTag(DishTag tag) => new() { Id = tag.Id, Title = tag.Title };

    public DishTag ToDishTag() => new() { Id = Id, Title = Title };
}
