using ElementLogiq.eGlobalShop.Billing.Domain;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Billing.Application.Data;

public interface IPaymentDbContext
{
    DbSet<Invoice> Invoice { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
