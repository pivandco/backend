namespace MerchandiseApi;

using FluentValidation;

public sealed class ProductCategoryValidator : AbstractValidator<ProductCategoryDto>
{
    public ProductCategoryValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
