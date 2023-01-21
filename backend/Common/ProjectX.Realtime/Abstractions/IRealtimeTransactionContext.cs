namespace ProjectX.Core.Realtime.Abstractions;

public interface IRealtimeTransactionContext
{
    void Add(RealtimeMessageContext message, IEnumerable<int> receivers);
    
    IEnumerable<(RealtimeIntegrationEvent, PublishProperties)> ExtractMessages();
}
