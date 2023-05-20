using task3_attempt2.Models;

namespace task3_attempt2.ViewModels;

public class CategoryOut
{
    public CategoryOut()
    {
    }

    public CategoryOut(Category category)
    {
        (Id, Name) = (category.Id, category.Name);
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}