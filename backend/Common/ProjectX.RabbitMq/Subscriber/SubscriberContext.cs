using ProjectX.RabbitMq.Pipeline;

namespace ProjectX.RabbitMq;

internal sealed class SubscriberContext
{
    public SubscriberContext(SubscriptionKey key,
                      Type eventType, 
                      IModel channel, 
                      QueueProperties queue, 
                      ExchangeProperties exchange, 
                      ConsumerProperties consumer,
                      Pipe.Handler<SubscriberRequest> handler)
    {
        Key = key;
        EventType = eventType.ThrowIfNull();
        Channel = channel.ThrowIfNull();
        Queue = queue.ThrowIfNull();
        Exchange = exchange.ThrowIfNull();
        Consumer = consumer.ThrowIfNull();
        Handler = handler.ThrowIfNull();
    }

    public SubscriptionKey Key { get; }

    public Type EventType { get; }

    public IModel Channel { get; }

    public QueueProperties Queue { get; }

    public ExchangeProperties Exchange { get; }

    public ConsumerProperties Consumer { get; }
      
    public Pipe.Handler<SubscriberRequest> Handler { get; }

    public override string ToString()
    {
        return $"{nameof(Queue)}: {Queue}, {nameof(Exchange)}: {Exchange}, {nameof(Consumer)}: {Consumer}, {nameof(EventType)}: {EventType.Name}";
    }
}
