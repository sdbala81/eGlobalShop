using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Billing.Application.Data;
using ElementLogiq.eGlobalShop.Billing.Domain;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Billing.Application.GetById;

public class GetCustomerByIdQueryHandler(IPaymentDbContext paymentDbContext) : IQueryHandler<GetPaymentByIdQuery, GetPaymentResponse>
{
    public async Task<Result<GetPaymentResponse>> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
    {
        var payment = await paymentDbContext.Payment.Where(c => c.Id == query.PaymentId)
            .Select(c => new GetPaymentResponse
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
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return payment is null ? Result.Failure<GetPaymentResponse>(PaymentErrors.NotFound(query.PaymentId)) : Result.Success(payment);
    }
}
