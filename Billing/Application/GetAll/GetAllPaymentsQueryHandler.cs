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
            .Invoice
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
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success<IReadOnlyList<GetPaymentResponse>>(payments);
    }
}
