using FluentValidation;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Create;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(500);
        RuleFor(p => p.Price)
            .GreaterThan(0);
        RuleFor(p => p.StockQuantity);

        RuleFor(p => p.CategoryId)
            .NotEmpty();
    }
}
