using FluentValidation;

namespace task3_attempt2.ViewModels;

public class CategoryValidator : AbstractValidator<CategoryIn>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}