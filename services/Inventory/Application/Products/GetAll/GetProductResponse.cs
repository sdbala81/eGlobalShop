using ElementLogiq.eGlobalShop.Inventory.Domain;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.GetAll;

public class GetProductResponse
{
    public Guid Id { get; set; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public Category Category { get; init; } // Navigation property
}
