using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NATS.Client.Core;
using NATS.Net;

namespace ElementLogiq.eGlobalShop.Nats.Helpers;

public class NatsDispatcherService(
    ILogger<NatsDispatcherService> logger,
    NatsClient natsClient,
    IServiceScopeFactory scopeFactory,
    IEnumerable<Type> subscriptionTypes // Change: inject types, not instances
) : BackgroundService
{
    private readonly ILogger _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting NATS dispatcher");

        var tasks = subscriptionTypes.Select(type => ProcessStream(type, stoppingToken)).ToArray();

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private async Task ProcessStream(Type subscriptionType, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var subscription = (INatsSubscription)scope.ServiceProvider.GetRequiredService(subscriptionType);

        await foreach (var natsMessage in natsClient.SubscribeAsync<string>(subscription.Subject, cancellationToken: cancellationToken)
                           .ConfigureAwait(false))
        {
            _logger.LogInformation("Received on {Subject}: {Data}", subscription.Subject, natsMessage.Data);

            try
            {
                await subscription.HandleAsync(natsMessage.Data!, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (NatsException ex)
            {
                _logger.LogError(ex, "Error handling subject {Subject}", subscription.Subject);
            }
        }
    }
}
