using ElementLogiq.eGlobalShop.Inventory.Application.Products.Update;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.WebApi.Endpoints;

public sealed class UpdateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/products",
                async (UpdateProductCommand updateProductCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(updateProductCommand, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Products");
    }
}
