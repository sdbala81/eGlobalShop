using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Orders.Domain;

public class Order : Entity
{
    public Guid Id { get; init; }

    public DateOnly OrderDate { get; init; }

    public Guid CustomerId { get; init; }

    public string Status { get; init; }

    public string ShippingAddress { get; init; }

    public string BillingAddress { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public string DiscountCode { get; init; }

    public ICollection<OrderItem> OrderItems { get; init; } = [];
}
