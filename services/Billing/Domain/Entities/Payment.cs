using ElementLogiq.eGlobalShop.Billing.Domain.Enums;

namespace ElementLogiq.eGlobalShop.Billing.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid CustomerId { get; set; }

    public BillingAddress BillingAddress { get; set; }
    public Guid OrderId { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentType PaymentType { get; set; }

    public PaymentStatus PaymentStatus { get; private set; } = PaymentStatus.Pending;

    public DateTime PaymentDateTime { get; private set; } = DateTime.UtcNow;

    public void MarkAuthorized()
    {
        PaymentStatus = PaymentStatus.Authorized;
    }

    public void MarkCaptured()
    {
        PaymentStatus = PaymentStatus.Captured;
    }

    public void MarkFailed()
    {
        PaymentStatus = PaymentStatus.Failed;
    }

    public void MarkRefunded()
    {
        PaymentStatus = PaymentStatus.Refunded;
    }

    public static Payment Create(Guid orderId, decimal amount, PaymentType paymentType)
    {
        return new Payment
        {
            OrderId = orderId,
            Amount = amount,
            PaymentType = paymentType,
            PaymentStatus = PaymentStatus.Pending,
            PaymentDateTime = DateTime.UtcNow,
            Id = Guid.NewGuid()
        };
    }
}
