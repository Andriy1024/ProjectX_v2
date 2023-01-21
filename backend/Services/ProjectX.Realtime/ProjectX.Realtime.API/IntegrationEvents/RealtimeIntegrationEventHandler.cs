using ProjectX.Core.Events.IntegrationEvent;
using ProjectX.Core.Realtime.Models;
using ProjectX.RabbitMq;
using ProjectX.Realtime.API.WebSockets;

namespace ProjectX.Realtime.IntegrationEvent;

/// <summary>
/// Handle integration events from RabbitMQ
/// </summary>
public sealed class RealtimeMessageDispatcher : IMessageDispatcher
{
    private readonly ApplicationWebSocketManager _webSocketManager;

    public RealtimeMessageDispatcher(ApplicationWebSocketManager webSocketManager)
    {
        _webSocketManager = webSocketManager;
    }

    public async Task HandleAsync<T>(T integrationEvent) where T : IIntegrationEvent
    {
        if(integrationEvent is RealtimeIntegrationEvent message) 
        {
            //await _webSocketManager.SendAsync(message.Message, message.Receivers);
            
            await _webSocketManager.SendAsync(message.Message, message.Receivers);
        }
    }
}
