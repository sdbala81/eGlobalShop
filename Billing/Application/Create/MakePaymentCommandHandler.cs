using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Billing.Application.Data;
using ElementLogiq.eGlobalShop.Billing.Domain.Entities;
using ElementLogiq.eGlobalShop.Billing.Domain.Enums;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Billing.Application.Create;

public class MakePaymentCommandHandler(IPaymentDbContext paymentDbContext) : ICommandHandler<MakePaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(MakePaymentCommand command, CancellationToken cancellationToken)
    {
        // Using C# 12/.NET 8+ Enum.Parse<TEnum>(string, bool ignoreCase = false)
        var payment = Payment.Create(command.OrderId, command.OrderAmount, Enum.Parse<PaymentType>(command.PaymentType));

        paymentDbContext.Payment.Add(payment);

        await paymentDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return payment.Id;
    }
}
