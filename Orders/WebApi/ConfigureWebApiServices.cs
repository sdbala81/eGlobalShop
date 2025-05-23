using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

namespace ElementLogiq.eGlobalShop.Orders.WebApi;

public static class ConfigureWebApiServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Add services to the container
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
}
