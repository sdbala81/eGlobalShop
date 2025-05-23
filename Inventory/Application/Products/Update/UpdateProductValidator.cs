using ElementLogiq.eGlobalShop.Inventory.Application.Products.Create;

using FluentValidation;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Update;

public class UpdateProductValidator : AbstractValidator<CreateProductCommand>
{
    public UpdateProductValidator()
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
