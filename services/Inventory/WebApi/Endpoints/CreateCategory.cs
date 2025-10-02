using ElementLogiq.eGlobalShop.Inventory.Application.Categories.Create;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.WebApi.Endpoints;

public sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/categories",
                async (CreateCategoryCommand createCategoryCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(createCategoryCommand, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Categories");
    }
}
