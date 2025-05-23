using FluentValidation;

namespace ElementLogiq.eGlobalShop.Shipping.Application.Ship;

public class ShipOrderValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");

        RuleFor(x => x.CustomerFullName)
            .NotEmpty().WithMessage("CustomerFullName is required.");

        RuleFor(x => x.CustomerContact)
            .NotEmpty().WithMessage("CustomerContact is required.");

        RuleFor(x => x.ShippingMethodType)
            .NotEmpty().WithMessage("PaymentType is required.");

        RuleFor(x => x.HouseNo)
            .NotEmpty().WithMessage("HouseNo is required.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("PostalCode is required.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(x => x.ShippingMethodType)
            .NotEmpty().WithMessage("ShippingMethodType is required.");
    }
}
