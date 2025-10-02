using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Update;

public sealed class UpdateProductCommand : ICommand<Guid>
{
    public Guid ProductId { get; set; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public int Quantity { get; set; }

    public byte CategoryId { get; set; }
}
