using ElementLogiq.eGlobalShop.Customers.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace ElementLogiq.eGlobalShop.Customers.WebApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();

        if (!dbContext.Database.GetAppliedMigrations()
                .Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
