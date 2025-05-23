using ElementLogiq.eGlobalShop.Shipping.Domain.Enums;

namespace ElementLogiq.eGlobalShop.Shipping.Domain.Entities;

public class Shipment
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid OrderId { get; private set; }

    public string CustomerFullName { get; set; }

    public string CustomerContact { get; set; }

    public ShippingAddress ShippingAddress { get; private set; }

    public ShippingMethodType ShippingMethodType { get; private set; }

    public ShipmentStatus Status { get; private set; } = ShipmentStatus.Pending;

    public string? TrackingNumber { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime? ShippedAt { get; private set; }

    public DateTime? DeliveredAt { get; private set; }

    private Shipment(Guid orderId, ShippingAddress shippingAddress, ShippingMethodType shippingMethodType)
    {
        OrderId = orderId;
        ShippingAddress = shippingAddress;
        ShippingMethodType = shippingMethodType;
        // Id and CreatedAt are initialized inline
    }

    public static Shipment Create(
        Guid orderId,
        ShippingAddress shippingAddress,
        ShippingMethodType shippingMethodType,
        string customerFullName,
        string customerContact)
    {
        var shipment = new Shipment(orderId, shippingAddress, shippingMethodType)
        {
            CustomerFullName = customerFullName,
            CustomerContact = customerContact
            // TrackingNumber is not set here
        };
        return shipment;
    }

    public void MarkShipped(string trackingNumber)
    {
        Status = ShipmentStatus.Shipped;
        TrackingNumber = trackingNumber;
        ShippedAt = DateTime.UtcNow;
    }

    public void MarkInTransit()
    {
        if (Status == ShipmentStatus.Shipped)
        {
            Status = ShipmentStatus.InTransit;
        }
    }

    public void MarkDelivered()
    {
        Status = ShipmentStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
    }

    public void MarkFailed()
    {
        Status = ShipmentStatus.Failed;
    }
}
