using ElementLogiq.eGlobalShop.Inventory.Application.Data;
using ElementLogiq.eGlobalShop.Inventory.Domain;
using ElementLogiq.eGlobalShop.Inventory.Infrastructure.Database;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElementLogiq.eGlobalShop.Inventory.Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration);
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<InventoryDbContext>(options => options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.DEFAULT))
            .UseSeeding((context, seeder) =>
            {
                context.Set<Category>()
                    .AddRange(InventorySeeder.Categories);

                context.Set<Product>()
                    .AddRange(InventorySeeder.Products);

                context.SaveChanges();
            })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IInventoryDbContext>(sp => sp.GetRequiredService<InventoryDbContext>());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }
}
