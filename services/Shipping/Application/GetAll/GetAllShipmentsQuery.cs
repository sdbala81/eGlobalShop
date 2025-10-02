using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Shipping.Application.GetById;

namespace ElementLogiq.eGlobalShop.Shipping.Application.GetAll;

public sealed record GetAllShipmentQuery : IQuery<IReadOnlyList<GetShipmentResponse>>;
