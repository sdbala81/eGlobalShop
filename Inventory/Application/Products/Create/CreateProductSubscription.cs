using System.Text.Json;

using ElementLogiq.eGlobalShop.Nats.Helpers;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Create;

public class CreateProductSubscription(ISender sender) : INatsSubscription
{
    public string Subject { get { return "products.create"; } }

    public async Task HandleAsync(string natsPayload, CancellationToken cancellationToken)
    {
        var createProductRequest = JsonSerializer.Deserialize<CreateProductCommand>(natsPayload)!;

        await sender.Send(createProductRequest, cancellationToken)
            .ConfigureAwait(false);
    }
}
