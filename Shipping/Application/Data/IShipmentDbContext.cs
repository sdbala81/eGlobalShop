using ElementLogiq.eGlobalShop.Shipping.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Shipping.Application.Data;

public interface IShipmentDbContext
{
    DbSet<Shipment> Shipment { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
