using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Create;

public sealed class CreateProductCommand : ICommand<Guid>
{
    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public byte CategoryId { get; set; }
}
