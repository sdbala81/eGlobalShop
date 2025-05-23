using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Billing.Application.Create;

public sealed record CreatePaymentCommand : ICommand<Guid>
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string PhoneNumber { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public string Address { get; init; }

    public string City { get; init; }

    public string State { get; init; }

    public string ZipCode { get; init; }
}
