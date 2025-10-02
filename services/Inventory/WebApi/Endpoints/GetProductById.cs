using ElementLogiq.eGlobalShop.Inventory.Application.Products.GetById;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.WebApi.Endpoints;

public class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/{productId:Guid}",
                async (Guid productId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetProductByIdQuery(productId);

                    var result = await sender.Send(query, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Customers");
    }
}
