using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Shipping.Application.Ship;

public sealed record ShipOrderCommand : ICommand<Guid>
{
    public Guid OrderId { get; init; }

    public string HouseNo { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string ShippingMethodType { get; init; }

    public string CustomerFullName { get; init; }

    public string CustomerContact { get; init; }
}
