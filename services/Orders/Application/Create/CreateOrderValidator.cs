using FluentValidation;

namespace ElementLogiq.eGlobalShop.Orders.Application.Create;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required.");

        RuleFor(x => x.OrderDate)
            .NotEmpty()
            .WithMessage("Order date is required.");

        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .WithMessage("Shipping address is required.")
            .MaximumLength(250);

        RuleFor(x => x.BillingAddress)
            .NotEmpty()
            .WithMessage("Billing address is required.")
            .MaximumLength(250);

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required.");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new CreateOrderItemValidator());
    }
}

public class CreateOrderItemValidator : AbstractValidator<CreateOrderItem>
{
    public CreateOrderItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
