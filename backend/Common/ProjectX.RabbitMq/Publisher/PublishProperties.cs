namespace ProjectX.RabbitMq;

public record PublishProperties 
{
    public ExchangeProperties Exchange { get; }

    public string RoutingKey { get; set; } = "";

    public bool EnableRetryPolicy { get; set; }

    public PublishProperties()
        : this(new ExchangeProperties())
    {
    }

    public PublishProperties(Exchange.Name exchangeName)
        : this(new ExchangeProperties(exchangeName))
    {
    }

    public PublishProperties(ExchangeProperties exchange)
    {
        Exchange = exchange.ThrowIfNull();
    }

    public static PublishProperties Validate(PublishProperties properties, bool allowEmptyRoutingKey = false) 
    {
        properties.ThrowIfNull();

        ExchangeProperties.Validate(properties.Exchange);

        if (!allowEmptyRoutingKey) 
        {
            properties.RoutingKey.ThrowIfNull();
        }
            
        return properties;
    }

    public static string CreateRoutingKey<T>(T integrationEvent)
        where T : IIntegrationEvent
    { 
        return integrationEvent.GetType().Name;
    }

    /// <summary>
    /// Need to investigate
    /// basicProperties.Persistent 
    /// </summary>
    //public bool Persistent { get; set; }
}
