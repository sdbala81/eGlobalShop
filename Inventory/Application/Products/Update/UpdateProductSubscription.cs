using System.Collections.ObjectModel;
using System.Text.Json;

using ElementLogiq.eGlobalShop.Nats.Helpers;

using MediatR;

namespace ElementLogiq.eGlobalShop.Inventory.Application.Products.Update;

public class UpdateProductSubscription(ISender sender) : INatsSubscription
{
    public string Subject { get { return "inventory.product.update"; } }

    public async Task HandleAsync(string natsPayload, CancellationToken cancellationToken)
    {
        var updateProductCommands = JsonSerializer.Deserialize<Collection<ProductToBeUpdated>>(natsPayload)!;

        await sender.Send(
                new UpdateProductsQuantityCommand
                {
                    ProductToBeUpdated = updateProductCommands
                },
                cancellationToken)
            .ConfigureAwait(false);
    }
}
