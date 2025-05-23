using ElementLogiq.eGlobalShop.Orders.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElementLogiq.eGlobalShop.Orders.Infrastructure.Database.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderDate)
            .IsRequired();
        builder.Property(o => o.CustomerId)
            .IsRequired();
        builder.Property(o => o.Status)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(o => o.ShippingAddress)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(o => o.BillingAddress)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(o => o.PaymentMethod)
            .IsRequired();
        builder.Property(o => o.DiscountCode)
            .HasMaxLength(50)
            .IsRequired(false);

        // Configure the one-to-many relationship with OrderItem
        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
