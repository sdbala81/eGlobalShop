using System.Collections.ObjectModel;

using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Orders.Application.Create;

public sealed record CreateOrderCommand : ICommand<Guid>
{
    public Guid CustomerId { get; init; }

    public DateOnly OrderDate { get; init; }

    public string ShippingAddress { get; init; }

    public string BillingAddress { get; init; }

    public string PaymentMethod { get; init; }

    public string DiscountCode { get; init; }

    public Collection<CreateOrderItem> OrderItems { get; init; }
}

public sealed record CreateOrderItem
{
    public Guid ProductId { get; init; }

    public int Quantity { get; init; }
}
