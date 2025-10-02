using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Inventory.Domain;

public class Category : Entity
{
    public byte Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public ICollection<Product> Products { get; init; } // Navigation property
}
