﻿using System.Net;

namespace ProjectX.Realtime.API.WebSockets;

public sealed class WebSocketMiddleware
{
    private const string ConnectionIdQueryParamName = "connectionId";

    private readonly ApplicationWebSocketManager _connectionManager;
    private readonly WebSocketAuthenticationManager _authenticationManager;

    public WebSocketMiddleware(RequestDelegate next,
           ApplicationWebSocketManager connectionManager,
           WebSocketAuthenticationManager authenticationManager)
    {
        _connectionManager = connectionManager;
        _authenticationManager = authenticationManager;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest) return;

        var cancellationToken = context.RequestAborted;

        ConnectionId? connectionId = GetConnectionId(context);

        if (!connectionId.HasValue)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        if (!_authenticationManager.Validate(connectionId.Value, out long userId))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        await _connectionManager.HandleAsync(connectionId.Value, userId, webSocket, cancellationToken);
    }

    private static ConnectionId? GetConnectionId(HttpContext context)
    {
        try
        {
            var connectionIdParameter = context.Request.Query.FirstOrDefault(t => string.Equals(t.Key, ConnectionIdQueryParamName, StringComparison.CurrentCultureIgnoreCase));

            ConnectionId connectionId = connectionIdParameter.Value.ToString();

            return connectionId;
        }
        catch
        {
        }

        return null;
    }
}
