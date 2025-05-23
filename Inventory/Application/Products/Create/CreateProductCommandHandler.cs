using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Inventory.Application.Data;
using ElementLogiq.eGlobalShop.Inventory.Domain;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Create;

public class CreateProductCommandHandler(IInventoryDbContext inventoryDbContext) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.StockQuantity,
            CategoryId = request.CategoryId
        };

        inventoryDbContext.Products.Add(product);

        await inventoryDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return product.Id;
    }
}
