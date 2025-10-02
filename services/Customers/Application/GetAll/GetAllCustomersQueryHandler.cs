using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Customers.Application.Data;
using ElementLogiq.eGlobalShop.Customers.Application.GetById;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Customers.Application.GetAll;

public class GetAllCustomersQueryHandler(ICusomerDbContext customerDbContext)
    : IQueryHandler<GetAllCustomersQuery, IReadOnlyList<GetCustomerResponse>>
{
    public async Task<Result<IReadOnlyList<GetCustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await customerDbContext
            .Customers
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
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success<IReadOnlyList<GetCustomerResponse>>(customers);
    }
}
