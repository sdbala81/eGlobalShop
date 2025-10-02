using ElementLogiq.eGlobalShop.Inventory.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Inventory.WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        if (!dbContext.Database.GetAppliedMigrations()
                .Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
