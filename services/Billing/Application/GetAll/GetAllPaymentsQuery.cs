using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Billing.Application.GetById;

namespace ElementLogiq.eGlobalShop.Billing.Application.GetAll;

public sealed record GetAllPaymentsQuery : IQuery<IReadOnlyList<GetPaymentResponse>>;
