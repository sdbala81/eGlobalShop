using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Inventory.Application.Data;
using ElementLogiq.eGlobalShop.Inventory.Domain;
using ElementLogiq.SharedKernel;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Update;

public class UpdateProductQuantityCommandHandler(IInventoryDbContext inventoryDbContext)
    : ICommandHandler<UpdateProductsQuantityCommand, List<Guid>>
{
    public async Task<Result<List<Guid>>> Handle(UpdateProductsQuantityCommand request, CancellationToken cancellationToken)
    {
        // Loop over each product in the request
        foreach (var productToUpdate in request.ProductToBeUpdated)
        {
            // Get the product from the database using ProductId from the request
            var product = await inventoryDbContext.Products.FindAsync(
                    [productToUpdate.ProductId],
                    cancellationToken)
                .ConfigureAwait(false);

            if (product == null)
            {
                return Result.Failure<List<Guid>>(InventoryErrors.NotFound(productToUpdate.ProductId));
            }

            // Reduce the quantity in the request from that in the database
            product.Quantity -= productToUpdate.Quantity;
        }

        await inventoryDbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success(
            request.ProductToBeUpdated.Select(p => p.ProductId)
                .ToList());
    }
}
