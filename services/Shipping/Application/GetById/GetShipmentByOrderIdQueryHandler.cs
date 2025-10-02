using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Shipping.Application.Data;
using ElementLogiq.eGlobalShop.Shipping.Domain;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Shipping.Application.GetById;

public class GetCustomerByIdQueryHandler(IShipmentDbContext shipmentDbContext) : IQueryHandler<GetShipmentByOrderIdQuery, GetShipmentResponse>
{
    public async Task<Result<GetShipmentResponse>> Handle(GetShipmentByOrderIdQuery query, CancellationToken cancellationToken)
    {
        var payment = await shipmentDbContext.Shipment.Where(c => c.Id == query.PaymentId)
            .Select(shipment => new GetShipmentResponse
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
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return payment is null ? Result.Failure<GetShipmentResponse>(PaymentErrors.NotFound(query.PaymentId)) : Result.Success(payment);
    }
}
