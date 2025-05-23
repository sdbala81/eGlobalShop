using ElementLogiq.eGlobalShop.WebApi.Helpers.Middleware;

using Microsoft.AspNetCore.Builder;

namespace ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}
