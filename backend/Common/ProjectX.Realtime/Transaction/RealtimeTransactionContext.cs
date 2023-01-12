namespace ProjectX.Core.Realtime.Transaction;

public sealed class RealtimeTransactionContext : IRealtimeTransactionContext
{
    private readonly Queue<(RealtimeIntegrationEvent, PublishProperties)> _messages =
                 new Queue<(RealtimeIntegrationEvent, PublishProperties)>();

    public void Add(RealtimeMessageContext message, IEnumerable<long> receivers)
    {
        message.ThrowIfNull();
        receivers.ThrowIfNull();

        var publishProperties = new PublishProperties(
                                    new ExchangeProperties(
                                        name: Exchange.Name.Realtime,
                                        type: Exchange.Type.Fanout,
                                        autoDelete: true,
                                        durable: false));

        var integrationEvent = new RealtimeIntegrationEvent(Guid.NewGuid(), message, receivers);

        _messages.Enqueue((integrationEvent, publishProperties));
    }

    public IEnumerable<(RealtimeIntegrationEvent, PublishProperties)> ExtractMessages()
    {
        while (_messages.TryDequeue(out var message))
        {
            yield return message;
        }
    }
}
