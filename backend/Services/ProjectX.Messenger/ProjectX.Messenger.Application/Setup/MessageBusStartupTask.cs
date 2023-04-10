using ProjectX.Core.StartupTasks;
using ProjectX.RabbitMq;

namespace ProjectX.Messenger.Application.Setup;

public sealed class MessageBusStartupTask : IStartupTask
{
    private readonly IRabbitMqConnectionService _rabbitMq;

    public MessageBusStartupTask(IRabbitMqConnectionService rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (!_rabbitMq.TryConnect()) 
        {
            throw new Exception("Rabbitmq unreachable.");
        }

        return Task.CompletedTask;
    }
}