using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Orders.Application.Data;
using ElementLogiq.eGlobalShop.Orders.Domain;
using ElementLogiq.SharedKernel;

using NATS.Net;

namespace ElementLogiq.eGlobalShop.Orders.Application.Create;

public class CreateOrderCommandHandler(IOrderDbContext orderDbContext, NatsClient natsClient) : ICommandHandler<CreateOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            CustomerId = command.CustomerId,
            OrderDate = command.OrderDate,
            ShippingAddress = command.ShippingAddress,
            BillingAddress = command.BillingAddress,
            PaymentMethod = Enum.Parse<PaymentMethod>(command.PaymentMethod, true),
            DiscountCode = command.DiscountCode,
            Status = "Pending"
        };

        orderDbContext.Orders.Add(order);

        await orderDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        var orderItems = command.OrderItems
            .Select(item => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            })
            .ToList();

        orderDbContext.OrderItems.AddRange(orderItems);

        await orderDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        await natsClient.PublishAsync(
                "inventory.product.update",
                orderItems,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return order.Id;
    }
}
