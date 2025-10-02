namespace ElementLogiq.eGlobalShop.Shipping.Domain.Enums;

public enum ShipmentStatus
{
    Pending,   // Created but not yet handed to carrier
    Shipped,   // Label generated, carrier picked up
    InTransit, // On the way
    Delivered, // Delivered to customer
    Failed
}
