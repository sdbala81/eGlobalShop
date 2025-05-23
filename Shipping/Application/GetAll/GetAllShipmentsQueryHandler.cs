using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Shipping.Application.Data;
using ElementLogiq.eGlobalShop.Shipping.Application.GetById;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Shipping.Application.GetAll;

public class GetAllShipmentsQueryHandler(IShipmentDbContext shipmentDbContext)
    : IQueryHandler<GetAllShipmentQuery, IReadOnlyList<GetShipmentResponse>>
{
    public async Task<Result<IReadOnlyList<GetShipmentResponse>>> Handle(GetAllShipmentQuery request, CancellationToken cancellationToken)
    {
        var payments = await shipmentDbContext
            .Shipment
            .Select(
                shipment => new GetShipmentResponse
                {
                    Id = shipment.Id,
                    OrderId = shipment.OrderId,
                    CustomerFullName = shipment.CustomerFullName,
                    CustomerContact = shipment.CustomerContact,
                    ShippingAddress = shipment.ShippingAddress.ToString(),
                    ShippingMethodType = shipment.ShippingMethodType.ToString(),
                    Status = shipment.Status.ToString(),
                    TrackingNumber = shipment.TrackingNumber!,
                    CreatedAt = shipment.CreatedAt,
                    ShippedAt = shipment.ShippedAt,
                    DeliveredAt = shipment.DeliveredAt
                })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success<IReadOnlyList<GetShipmentResponse>>(payments);
    }
}
