namespace ElementLogiq.eGlobalShop.Nats.Helpers;

public interface INatsSubscription
{
    string Subject { get; }

    Task HandleAsync(string natsPayload, CancellationToken cancellationToken);
}
