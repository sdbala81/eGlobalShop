using ElementLogiq.eGlobalShop.Shipping.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Shipping.WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<ShipmentDbContext>();

        if (!dbContext.Database.GetAppliedMigrations()
                .Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
