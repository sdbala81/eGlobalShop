
using ElementLogiq.eGlobalShop.Billing.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElementLogiq.eGlobalShop.Billing.Infrastructure.Database.Configuration;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Domain.Invoice> builder)
    {
        builder.HasKey(c => c.Id);
    }
}
