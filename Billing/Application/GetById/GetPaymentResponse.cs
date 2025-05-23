namespace ElementLogiq.eGlobalShop.Billing.Application.GetById;

public class GetPaymentResponse
{
    public Guid Id { get; set; }


    // Payment entity properties
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentType { get; set; } // Changed from string to PaymentType
    public string Status { get; set; } // Changed from string to PaymentStatus
    public DateTime PaymentDateTime { get; set; }
    public string BillingAddress { get; set; } // Changed from string to BillingAddress
}
