using ElementLogiq.eGlobalShop.Customers.Domain;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Customers.Application.Data;

public interface ICusomerDbContext
{
    DbSet<Customer> Customers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
