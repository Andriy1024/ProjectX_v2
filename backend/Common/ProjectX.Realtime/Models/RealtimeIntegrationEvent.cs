using ProjectX.Core.Events.IntegrationEvent;

namespace ProjectX.Core.Realtime.Models;

public class RealtimeIntegrationEvent : ITransientIntegrationEvent
{
    public RealtimeMessageContext Message { get; set; }

    public IEnumerable<int> Receivers { get; set; }

    public RealtimeIntegrationEvent()
    {
    }

    public RealtimeIntegrationEvent(RealtimeMessageContext message, IEnumerable<int> receivers)
    {
        Message = message;
        Receivers = receivers;
    }
}
