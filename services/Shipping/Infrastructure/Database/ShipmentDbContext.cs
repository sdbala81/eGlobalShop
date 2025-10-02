using ElementLogiq.eGlobalShop.Shipping.Application.Data;
using ElementLogiq.eGlobalShop.Shipping.Domain.Entities;
using ElementLogiq.SharedKernel;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Shipping.Infrastructure.Database;

public sealed class ShipmentDbContext(DbContextOptions<ShipmentDbContext> options, IPublisher publisher) : DbContext(options),
    IShipmentDbContext
{
    public DbSet<Shipment> Shipment { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        var result = await base.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        await PublishDomainEventsAsync()
            .ConfigureAwait(false);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipmentDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.DEFAULT);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent)
                .ConfigureAwait(false);
        }
    }
}
