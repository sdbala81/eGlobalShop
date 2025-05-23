using ElementLogiq.eGlobalShop.Shipping.Application.Ship;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Shipping.WebApi.Endpoints;

public sealed class ShipOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/shipments",
                async (ShipOrderCommand shipOrderCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(shipOrderCommand, cancellationToken)
     .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Shipment");
    }
}
