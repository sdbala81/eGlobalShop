using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Messaging;
using ElementLogiq.eGlobalShop.Inventory.Application.Data;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.GetAll;

public class GetAllProductsQueryHandler(IInventoryDbContext inventoryDbContext)
    : IQueryHandler<GetAllProductsQuery, IReadOnlyList<GetProductResponse>>
{
    public async Task<Result<IReadOnlyList<GetProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var customers = await inventoryDbContext
            .Products
            .Select(c => new GetProductResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Price = c.Price,
                StockQuantity = c.Quantity
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success<IReadOnlyList<GetProductResponse>>(customers);
    }
}
