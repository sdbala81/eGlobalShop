using FluentValidation;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Categories.Create;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(500);
    }
}
