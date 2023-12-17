﻿using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;

namespace ProjectX.Realtime.Websocket;

public static class WebSocketExtensions
{
    public static bool IsOpen([NotNullWhen(true)] this WebSocket? webSocket)
        => webSocket?.State is WebSocketState.Open;

    public static bool IsClosed([NotNullWhen(false)] this WebSocket? webSocket)
        => !IsOpen(webSocket);
}