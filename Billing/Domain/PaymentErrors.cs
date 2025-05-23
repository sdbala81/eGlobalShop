using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Billing.Domain;

public static class PaymentErrors
{
    public static Error NotFound(Guid paymentId)
    {
        return Error.NotFound("Payment.NotFound", $"The payment with the Id = '{paymentId}' was not found");
    }
}
