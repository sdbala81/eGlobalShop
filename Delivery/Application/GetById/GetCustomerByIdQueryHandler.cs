using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Customers.Application.Data;
using ElementLogiq.eGlobalShop.Customers.Domain;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Customers.Application.GetById;

public class GetCustomerByIdQueryHandler(ICusomerDbContext customerDbContext) : IQueryHandler<GetCustomerByIdQuery, GetCustomerResponse>
{
    public async Task<Result<GetCustomerResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await customerDbContext.Customers.Where(c => c.Id == query.CustomerId)
            .Select(c => new GetCustomerResponse
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

        return customer is null ? Result.Failure<GetCustomerResponse>(CustomerErrors.NotFound(query.CustomerId)) : Result.Success(customer);
    }
}
