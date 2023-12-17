using static System.Threading.Channels.Channel;

namespace ProjectX.Realtime.PubSub.Topic;


/// <summary>
/// The in-memory pub/sub topic representation.
/// </summary>
/// <typeparam name="TMessage">
/// The message type used with this topic.
/// </typeparam>
internal sealed class InMemoryTopic<TMessage> : DefaultTopic<TMessage>
{
    public InMemoryTopic(
        string name,
        int capacity,
        TopicBufferFullMode fullMode)
        : base(
            name,
            capacity,
            fullMode,
            CreateUnbounded<TMessage>())
    {
    }
}
