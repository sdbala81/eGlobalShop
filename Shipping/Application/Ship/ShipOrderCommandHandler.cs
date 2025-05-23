using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Shipping.Application.Data;
using ElementLogiq.eGlobalShop.Shipping.Domain.Entities;
using ElementLogiq.eGlobalShop.Shipping.Domain.Enums;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Shipping.Application.Ship;

public class ShipOrderCommandHandler(IShipmentDbContext shipmentDbContext) : ICommandHandler<ShipOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        var shippingAddress = ShippingAddress.Create(
            command.HouseNo,
            command.Street,
            command.City,
            command.State,
            command.PostalCode,
            command.Country);

        var shipment = Shipment.Create(
            command.OrderId,
            shippingAddress,
            Enum.Parse<ShippingMethodType>(command.ShippingMethodType, true),
            command.CustomerFullName,
            command.CustomerContact);

        shipmentDbContext.Shipment.Add(shipment);

        await shipmentDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return shipment.Id;
    }
}
