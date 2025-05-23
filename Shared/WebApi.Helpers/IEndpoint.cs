using Microsoft.AspNetCore.Routing;

namespace ElementLogiq.eGlobalShop.WebApi.Helpers;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
