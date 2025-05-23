using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Behaviors;
using ElementLogiq.eGlobalShop.Nats.Helpers;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using NATS.Net;

namespace ElementLogiq.eGlobalShop.Inventory.Application;

public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ConfigureApplicationServices).Assembly);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ConfigureApplicationServices).Assembly, includeInternalTypes: true);

        var natsUrl = Environment.GetEnvironmentVariable("NATS_SERVER_URL");
        ArgumentNullException.ThrowIfNull(natsUrl);

        // Automatically register all types implementing INatsSubscription
        var subscriptionTypes = typeof(ConfigureApplicationServices).Assembly
            .GetTypes()
            .Where(t => typeof(INatsSubscription).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
            .ToList();

        foreach (var type in subscriptionTypes)
        {
            services.AddScoped(type);
        }

        services.AddSingleton<IEnumerable<Type>>(subscriptionTypes);

        services.AddHostedService<NatsDispatcherService>();

        services.AddSingleton(_ =>
            new NatsClient(natsUrl));

        return services;
    }
}
