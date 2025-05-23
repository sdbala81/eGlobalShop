using ElementLogiq.eGlobalShop.Orders.Domain;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Orders.Application.Data;

public interface IOrderDbContext
{
    DbSet<Order> Orders { get; }

    DbSet<OrderItem> OrderItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
