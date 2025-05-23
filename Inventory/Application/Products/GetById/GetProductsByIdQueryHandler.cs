using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Inventory.Application.Data;
using ElementLogiq.eGlobalShop.Inventory.Application.Products.GetAll;
using ElementLogiq.eGlobalShop.Inventory.Domain;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.GetById;

public class GetProductsByIdQueryHandler(IInventoryDbContext inventoryDbContext) : IQueryHandler<GetProductByIdQuery, GetProductResponse>
{
    public async Task<Result<GetProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var productResponse = await inventoryDbContext.Products.Where(c => c.Id == query.ProductId)
            .Select(c => new GetProductResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Price = c.Price,
                StockQuantity = c.Quantity
            })
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return productResponse is null
            ? Result.Failure<GetProductResponse>(InventoryErrors.NotFound(query.ProductId))
            : Result.Success(productResponse);
    }
}
