using ElementLogiq.eGlobalShop.Inventory.Domain;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Data;

public interface IInventoryDbContext
{
    DbSet<Product> Products { get; }

    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
