using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Billing.Application.GetById;

public record GetPaymentByIdQuery(Guid PaymentId) : IQuery<GetPaymentResponse>;
