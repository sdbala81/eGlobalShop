using ElementLogiq.eGlobalShop.Billing.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Billing.Application.Data;

public interface IPaymentDbContext
{
    DbSet<Payment> Payment { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
