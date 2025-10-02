using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Orders.Domain;

public class OrderItem : Entity
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public Guid OrderId { get; init; }

    public int Quantity { get; init; }
}
