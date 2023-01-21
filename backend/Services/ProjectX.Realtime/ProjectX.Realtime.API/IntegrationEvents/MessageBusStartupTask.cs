using ProjectX.Core.Realtime.Models;
using ProjectX.Core.StartupTasks;
using ProjectX.RabbitMq;

namespace ProjectX.Realtime.IntegrationEvent;

public sealed class MessageBusStartupTask : IStartupTask
{
    private readonly IRabbitMqSubscriber _messageBus;

    public MessageBusStartupTask(IRabbitMqSubscriber messageBus)
    {
        _messageBus = messageBus;
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _messageBus.Subscribe<RealtimeIntegrationEvent>(o => 
        {
            o.Exchange.Name = Exchange.Name.Realtime;
            o.Exchange.Type = Exchange.Type.Fanout;
            o.Consumer.Autoack = true;
            o.Consumer.RetryOnFailure = false;
        });

        return Task.CompletedTask;
    }
}
