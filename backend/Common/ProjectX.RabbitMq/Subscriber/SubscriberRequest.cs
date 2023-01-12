namespace ProjectX.RabbitMq.Pipeline;

public class SubscriberRequest
{
    public SubscriberRequest(BasicDeliverEventArgs eventArgs, IIntegrationEvent integrationEvent)
    {
        RabbitPrperties = eventArgs;
        IntegrationEvent = integrationEvent;
    }

    public BasicDeliverEventArgs RabbitPrperties { get; }

    public IIntegrationEvent IntegrationEvent { get; }
}
