using ProjectX.Core;

namespace ProjectX.Messenger.Persistence;

public interface IMessengerEventStore
{
    /// <summary>
    /// Store uncommited events.
    /// </summary>
    Task StoreAsync<T>(T aggregate) where T : IEventSourcedAggregate;

    /// <summary>
    /// Perform a live aggregation of the raw events in this stream to a T object.
    /// </summary>
    Task<T> LoadAsync<T>(string id) where T : IEventSourcedAggregate, new();
}