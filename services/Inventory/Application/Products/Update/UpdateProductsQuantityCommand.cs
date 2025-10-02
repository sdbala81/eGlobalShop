using System.Collections.ObjectModel;

using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Update;

public sealed class UpdateProductsQuantityCommand : ICommand<List<Guid>>
{
    public Collection<ProductToBeUpdated> ProductToBeUpdated { get; init; }
}

public sealed record ProductToBeUpdated(Guid ProductId, int Quantity);
