using ElementLogiq.eGlobalShop.Billing.Application.Pay;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Billing.WebApi.Endpoints;

public sealed class MakePayment : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/payments",
                async (MakePaymentCommand createPaymentCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(createPaymentCommand, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Payments");
    }
}
