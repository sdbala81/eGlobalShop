using ElementLogiq.eGlobalShop.Application.Helpers.Abstractions.Behaviors;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace ElementLogiq.eGlobalShop.Customers.Application;

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

        return services;
    }
}
