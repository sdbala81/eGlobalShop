using ElementLogiq.eGlobalShop.Billing.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Billing.WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

        if (!dbContext.Database.GetAppliedMigrations()
                .Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
