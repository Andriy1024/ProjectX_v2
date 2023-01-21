using ProjectX.Core.Events.IntegrationEvent;

namespace ProjectX.Core.Realtime.Models;

public class RealtimeIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; set; }

    public RealtimeMessageContext Message { get; set; }

    public IEnumerable<int> Receivers { get; set; }

    public RealtimeIntegrationEvent()
    {
    }

    public RealtimeIntegrationEvent(Guid id, RealtimeMessageContext message, IEnumerable<int> receivers)
    {
        Id = id;
        Message = message;
        Receivers = receivers;
    }
}
