namespace ProjectX.RabbitMq.Tracing;

internal static class TracingConstants
{
    #region RabbitMq

    public const string SystemKey = "messaging.system";

    public const string SystemValue = "rabbitmq";

    public const string DestinationKind = "messaging.destination_kind";

    public const string DestinationQueue = "queue";

    //key for exchange name
    public const string DestinationKey = "messaging.destination";

    public const string RoutingKey = "routing_key";
    
    #endregion

    public const string ActivityId = "activity-id";

    public const string TraceId = "trace-id";

    public const string CorrelationId = "correlation-id";

    //public const string MessageId = "message-id";
}