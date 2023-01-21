namespace ProjectX.RabbitMq.Pipeline;

public record SubscriberRequest(
    BasicDeliverEventArgs RabbitPrperties, 
    IIntegrationEvent IntegrationEvent);