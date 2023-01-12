namespace ProjectX.Core.Realtime.Abstractions;

public interface IRealtimeTransactionContext
{
    void Add(RealtimeMessageContext message, IEnumerable<long> receivers);
    IEnumerable<(RealtimeIntegrationEvent, PublishProperties)> ExtractMessages();
}
