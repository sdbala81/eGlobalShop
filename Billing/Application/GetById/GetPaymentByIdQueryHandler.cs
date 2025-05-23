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
        var payment = await paymentDbContext.Invoice.Where(c => c.Id == query.PaymentId)
            .Select(c => new GetPaymentResponse
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                DateOfBirth = c.DateOfBirth,
                Address = c.Address,
                City = c.City,
                State = c.State,
                ZipCode = c.ZipCode
            })
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return payment is null ? Result.Failure<GetPaymentResponse>(PaymentErrors.NotFound(query.PaymentId)) : Result.Success(payment);
    }
}
