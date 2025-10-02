using ElementLogiq.eGlobalShop.Orders.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElementLogiq.eGlobalShop.Orders.Infrastructure.Database.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        builder.Property(oi => oi.ProductId)
            .IsRequired();
        builder.Property(oi => oi.Quantity)
            .IsRequired();
        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.HasOne<Order>() // Explicitly define the relationship
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Ensure cascade delete is applied
    }
}
