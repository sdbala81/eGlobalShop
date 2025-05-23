using ElementLogiq.eGlobalShop.Orders.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Orders.WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        if (!dbContext.Database.GetAppliedMigrations()
                .Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
