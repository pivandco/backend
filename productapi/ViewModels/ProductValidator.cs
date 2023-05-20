using FluentValidation;

namespace task3_attempt2.ViewModels;

public class ProductValidator : AbstractValidator<ProductItemIn>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}