using System.Text.Json;

using ElementLogiq.eGlobalShop.Billing.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElementLogiq.eGlobalShop.Billing.Infrastructure.Database.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(c => c.CustomerId)
            .IsRequired();

        // ─── replace OwnsOne with a JSON converter ────────────────────────
        var addressConverter = new ValueConverter<BillingAddress, string>(
            billingAddress => JsonSerializer.Serialize(billingAddress, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<BillingAddress>(json, (JsonSerializerOptions?)null)!
        );

        builder.Property(c => c.BillingAddress)
            .HasConversion(addressConverter)
            .IsRequired();

        builder.Property(c => c.OrderId)
            .IsRequired();

        builder.Property(c => c.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.PaymentType)
            .IsRequired();

        builder.Property(c => c.PaymentStatus)
            .IsRequired();

        builder.Property(c => c.PaymentDateTime)
            .IsRequired();
    }
}
