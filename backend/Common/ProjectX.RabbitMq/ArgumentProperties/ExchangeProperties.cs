﻿namespace ProjectX.RabbitMq;

public record ExchangeProperties 
{
    public Exchange.Name Name { get; set; }

    public Exchange.Type Type { get; set; } = Exchange.Type.Direct;

    public bool AutoDelete { get; set; } = true;

    public bool Durable { get; set; } = false;

    public bool IsFanout => Type?.Value == Exchange.Type.Fanout;

    public ExchangeProperties() {}

    public ExchangeProperties(Exchange.Name name)
    {
        Name = name;
    }

    public ExchangeProperties(Exchange.Name name, Exchange.Type type, 
        bool autoDelete, bool durable) : this(name)
    {
        Type = type;
        AutoDelete = autoDelete;
        Durable = durable;
    }

    public static ExchangeProperties Validate(ExchangeProperties exchange) 
    {
        exchange.ThrowIfNull();
        exchange.Name.ThrowIfNull();
        exchange.Type.ThrowIfNull();

        return exchange;
    }
}
