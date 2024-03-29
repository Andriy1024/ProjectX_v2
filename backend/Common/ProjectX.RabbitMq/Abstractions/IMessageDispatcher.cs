﻿using ProjectX.RabbitMq.Pipeline;

namespace ProjectX.RabbitMq;

/// <summary>
/// Implementation of the interface is responsible for dispatching events received from messages broker.
/// </summary>
public interface IMessageDispatcher
{
    Task HandleAsync(SubscriberRequest integrationEvent);
}