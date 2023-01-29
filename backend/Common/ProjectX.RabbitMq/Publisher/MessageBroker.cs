using ProjectX.Core.Observability;

namespace ProjectX.RabbitMq.Publisher;

public interface IMessageBroker 
{
    void Publish<T>(T integrationEvent, PublishProperties properties) where T : IIntegrationEvent;

    void Publish<T>(T integrationEvent, Action<PublishProperties> action) where T : IIntegrationEvent;
}

public class MessageBroker : IMessageBroker
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;
    private readonly ITracer _tracer;

    public MessageBroker(IRabbitMqPublisher rabbitMqPublisher, ITracer tracer)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
        _tracer = tracer;
    }

    public void Publish<T>(T integrationEvent, PublishProperties properties) where T : IIntegrationEvent
    {
        _tracer.Trace(integrationEvent.GetType().Name, () => 
        {
            _rabbitMqPublisher.Publish(integrationEvent, properties);
            
            return Task.CompletedTask;
        });
    }

    public void Publish<T>(T integrationEvent, Action<PublishProperties> action) where T : IIntegrationEvent
    {
        var properties = new PublishProperties();

        action(properties);

        Publish(integrationEvent, properties);
    }
}