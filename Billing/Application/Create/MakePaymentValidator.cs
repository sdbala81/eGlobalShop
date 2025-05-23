using FluentValidation;

namespace ElementLogiq.eGlobalShop.Billing.Application.Create;

public class MakePaymentValidator : AbstractValidator<MakePaymentCommand>
{
    public MakePaymentValidator()
    {
        RuleFor(x => x.HouseNo)
            .NotEmpty().WithMessage("HouseNo is required.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("PostalCode is required.");

        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.PaymentType)
            .NotEmpty().WithMessage("PaymentType is required.");

        RuleFor(x => x.OrderAmount)
            .NotNull().WithMessage("OrderAmount is required.")
            .GreaterThan(1).WithMessage("OrderAmount must be greater than 1.");
    }
}
