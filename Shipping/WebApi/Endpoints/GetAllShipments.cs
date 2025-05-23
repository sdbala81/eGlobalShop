using ElementLogiq.eGlobalShop.Shipping.Application.GetAll;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Shipping.WebApi.Endpoints;

public class GetAllShipments : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/shipments",
                async (ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetAllShipmentQuery();

                    var result = await sender.Send(query, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Shipment");
    }
}
