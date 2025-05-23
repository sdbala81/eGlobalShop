using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

namespace ElementLogiq.eGlobalShop.Billing.WebApi;

public static class ConfigureWebApiServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
