using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Customers.Application.Data;
using ElementLogiq.eGlobalShop.Customers.Domain;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Customers.Application.Create;

public class CreateCustomerCommandHandler(ICusomerDbContext customerDbContext) : ICommandHandler<CreateCustomerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            DateOfBirth = command.DateOfBirth,
            Address = command.Address,
            City = command.City,
            State = command.State,
            ZipCode = command.ZipCode
        };

        customerDbContext.Customers.Add(customer);

        await customerDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return customer.Id;
    }
}
