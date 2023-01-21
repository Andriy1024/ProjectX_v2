namespace ProjectX.Core.Realtime.Abstractions;

/// <summary>
/// The context keeps realtime messages that should be published once transaction is completed.
/// </summary>
public interface IRealtimeTransactionContext
{
    void Add(RealtimeMessageContext message, IEnumerable<int> receivers);
    
    IEnumerable<(RealtimeIntegrationEvent, PublishProperties)> ExtractMessages();
}
