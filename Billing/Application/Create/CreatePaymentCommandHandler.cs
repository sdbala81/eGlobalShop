using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Billing.Application.Data;
using ElementLogiq.eGlobalShop.Billing.Domain;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Billing.Application.Create;

public class CreatePaymentCommandHandler(IPaymentDbContext paymentDbContext) : ICommandHandler<CreatePaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var invoice = new Invoice
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

        paymentDbContext.Invoice.Add(invoice);

        await paymentDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return invoice.Id;
    }
}
