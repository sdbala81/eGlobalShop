using ElementLogiq.eGlobalShop.Orders.Application.Create;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Orders.WebApi.Endpoints;

public sealed class CreateOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/orders",
                async (CreateOrderCommand createOrderCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(createOrderCommand, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Orders");
    }
}
