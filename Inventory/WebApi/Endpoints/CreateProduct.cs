using ElementLogiq.eGlobalShop.Inventory.Application.Products.Create;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.WebApi.Endpoints;

public sealed class CreateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/products",
                async (CreateProductCommand createProductCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(createProductCommand, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Products");
    }
}
