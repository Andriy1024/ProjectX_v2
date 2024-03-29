﻿namespace ProjectX.RabbitMq;

public interface IRabbitMqConnectionService : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateChannel();
}
