using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Inventory.Domain;

public class Product : Entity
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public int Quantity { get; set; }

    public Category Category { get; init; } // Navigation property

    public byte CategoryId { get; init; }
}
