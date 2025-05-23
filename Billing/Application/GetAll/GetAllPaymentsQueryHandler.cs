using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Billing.Application.Data;
using ElementLogiq.eGlobalShop.Billing.Application.GetById;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Billing.Application.GetAll;

public class GetAllPaymentsQueryHandler(IPaymentDbContext paymentDbContext)
    : IQueryHandler<GetAllPaymentsQuery, IReadOnlyList<GetPaymentResponse>>
{
    public async Task<Result<IReadOnlyList<GetPaymentResponse>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await paymentDbContext
            .Payment
            .Select(
                c => new GetPaymentResponse
                {
                    Id = c.Id,
                    CustomerId = c.CustomerId,
                    OrderId = c.OrderId,
                    Amount = c.Amount,
                    PaymentType = c.PaymentType.ToString(),
                    Status = c.PaymentStatus.ToString(),
                    PaymentDateTime = c.PaymentDateTime,
                    BillingAddress = c.BillingAddress.ToString()
                })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success<IReadOnlyList<GetPaymentResponse>>(payments);
    }
}
