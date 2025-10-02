using System.Text.Json;

using ElementLogiq.eGlobalShop.Shipping.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElementLogiq.eGlobalShop.Shipping.Infrastructure.Database.Configuration;

public class ShippmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        // ─── replace OwnsOne with a JSON converter ────────────────────────
        var addressConverter = new ValueConverter<ShippingAddress, string>(
            billingAddress => JsonSerializer.Serialize(billingAddress, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<ShippingAddress>(json, (JsonSerializerOptions?)null)!
        );

        builder.Property(c => c.ShippingAddress)
            .HasConversion(addressConverter)
            .IsRequired();

        builder.Property(c => c.OrderId)
            .IsRequired();

        builder.Property(c => c.CustomerFullName)
            .IsRequired(false);

        builder.Property(c => c.CustomerContact)
            .IsRequired(false);

        builder.Property(c => c.ShippingMethodType)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.TrackingNumber)
            .IsRequired(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.ShippedAt)
            .IsRequired(false);

        builder.Property(c => c.DeliveredAt)
            .IsRequired(false);
    }
}
