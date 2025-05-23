using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Billing.Application.Create;

public sealed record MakePaymentCommand : ICommand<Guid>
{
    public Guid OrderId { get; set; }

    public string PaymentType { get; set; }

    public decimal OrderAmount { get; set; }

    public Guid CustomerId { get; set; }

    public string HouseNo { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }
}
