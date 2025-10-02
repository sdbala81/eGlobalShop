using ElementLogiq.eGlobalShop.Customers.Application.Data;
using ElementLogiq.eGlobalShop.Customers.Domain;
using ElementLogiq.eGlobalShop.Customers.Infrastructure.Database;
using ElementLogiq.SharedKernel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElementLogiq.eGlobalShop.Customers.Infrastructure;

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

        services.AddDbContext<CustomerDbContext>(optionsBuilder => optionsBuilder.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.DEFAULT))
            .UseSeeding((context, seeder) =>
            {
                context.Set<Customer>()
                    .AddRange(SeedCustomers.Customers);

                context.SaveChanges();
            })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<ICusomerDbContext>(sp => sp.GetRequiredService<CustomerDbContext>());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }
}
