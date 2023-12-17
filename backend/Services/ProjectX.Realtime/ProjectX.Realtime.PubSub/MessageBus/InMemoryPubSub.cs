using ProjectX.Realtime.PubSub.Topic;

namespace ProjectX.Realtime.PubSub.MessageBus;

/// <summary>
/// The in-memory pub/sub provider implementation.
/// </summary>
internal sealed class InMemoryPubSub : DefaultPubSub
{
    private readonly SubscriptionOptions _options;

    public InMemoryPubSub(
        SubscriptionOptions options)
        : base(options)
    {
        _options = options;
    }

    protected override ValueTask OnSendAsync<TMessage>(
        string formattedTopic,
        TMessage message,
        CancellationToken cancellationToken = default)
    {
        if (TryGetTopic<TMessage>(formattedTopic, out var topic))
        {
            topic.Publish(message);
        }
        
        return ValueTask.CompletedTask;
    }

    protected override ValueTask OnCompleteAsync(string formattedTopic)
    {
        if (TryGetTopic(formattedTopic, out var topic))
        {
            topic.Complete();
        }

        return ValueTask.CompletedTask;
    }

    protected override DefaultTopic<TMessage> OnCreateTopic<TMessage>(
        string formattedTopic,
        int? bufferCapacity,
        TopicBufferFullMode? bufferFullMode)
        => new InMemoryTopic<TMessage>(
            formattedTopic,
            bufferCapacity ?? _options.TopicBufferCapacity,
            bufferFullMode ?? _options.TopicBufferFullMode);

    protected override string FormatTopicName(string topic)
        => topic;
}
