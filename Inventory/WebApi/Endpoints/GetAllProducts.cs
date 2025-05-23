using ElementLogiq.eGlobalShop.Inventory.Application.Products.GetAll;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.WebApi.Endpoints;

public class GetAllProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products",
                async (ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetAllProductsQuery();

                    var result = await sender.Send(query, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Products");
    }
}
