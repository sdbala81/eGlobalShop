using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Shipping.Application.GetById;

public record GetShipmentByOrderIdQuery(Guid PaymentId) : IQuery<GetShipmentResponse>;
