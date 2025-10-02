namespace ElementLogiq.eGlobalShop.Shipping.Application.GetById;

public class GetShipmentResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OrderId { get; set; }

    public string CustomerFullName { get; set; }

    public string CustomerContact { get; set; }

    public string ShippingAddress { get; set; }

    public string ShippingMethodType { get; set; }

    public string Status { get; set; }

    public string TrackingNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

}
