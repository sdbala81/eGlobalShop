using ElementLogiq.eGlobalShop.Customers.Application.Create;
using ElementLogiq.eGlobalShop.WebApi.Helpers;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;
using ElementLogiq.eGlobalShop.WebApi.Helpers.Infrastructure;

using MediatR;

namespace ElementLogiq.eGlobalShop.Customers.WebApi.Endpoints;

public sealed class CreateCustomer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/customers",
                async (CreateCustomerCommand createCustomerCommand, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(createCustomerCommand, cancellationToken)
                        .ConfigureAwait(false);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags("Customers");
    }
}
