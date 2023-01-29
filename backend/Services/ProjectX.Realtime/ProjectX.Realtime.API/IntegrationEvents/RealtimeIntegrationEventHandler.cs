using ProjectX.Core.Observability;
using ProjectX.Core.Realtime.Models;
using ProjectX.RabbitMq;
using ProjectX.RabbitMq.Pipeline;
using ProjectX.RabbitMq.Tracing;
using ProjectX.Realtime.API.WebSockets;
using ProjectX.Core.Context;

namespace ProjectX.Realtime.IntegrationEvent;

/// <summary>
/// Handle integration events from RabbitMQ
/// </summary>
public sealed class RealtimeMessageDispatcher : IMessageDispatcher
{
    private readonly ApplicationWebSocketManager _webSocketManager;
    private readonly ITracer _tracer;
    private readonly IContextAccessor _contextAccessor;

    public RealtimeMessageDispatcher(
        ApplicationWebSocketManager webSocketManager,
        ITracer tracer,
        IContextAccessor contextAccessor)
    {
        _webSocketManager = webSocketManager;
        _tracer = tracer;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(SubscriberRequest input)
    {
        if (input.IntegrationEvent is RealtimeIntegrationEvent message)
        {
            var context = input.RabbitPrperties.BasicProperties.ExtractContext();

            _contextAccessor.Context = context;

            await _tracer.Trace(nameof(RealtimeIntegrationEvent), () =>
            {
                return _webSocketManager.SendAsync(message.Message, message.Receivers);
            });
        }
    }
}