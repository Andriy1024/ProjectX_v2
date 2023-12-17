using static System.Net.Mime.MediaTypeNames;
using System.Buffers;
using System.Net.Http;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.Realtime.Websocket;

internal sealed class WebSocketConnection
{
    private WebSocket? _webSocket;
    private bool _disposed;

    public WebSocketConnection(HttpContext httpContext)
    {
        HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
    }

    public bool IsClosed => _webSocket.IsClosed();

    public HttpContext HttpContext { get; }

    public IServiceProvider RequestServices => HttpContext.RequestServices;

    public CancellationToken ApplicationStopping
        => RequestServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

    public CancellationToken RequestAborted => HttpContext.RequestAborted;

    public IDictionary<string, object?> ContextData { get; } = new Dictionary<string, object?>();

    public async Task TryAcceptConnection()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(WebSocketConnection));
        }

        _webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
    }

    public ValueTask SendAsync(
        ReadOnlyMemory<byte> message,
        CancellationToken cancellationToken = default)
    {
        var webSocket = _webSocket;

        if (_disposed || webSocket.IsClosed())
        {
            return default;
        }

        return webSocket.SendAsync(message, WebSocketMessageType.Text, true, cancellationToken);
    }

    public async Task<bool> ReadMessageAsync(
        IBufferWriter<byte> writer,
        CancellationToken cancellationToken = default)
    {
        var webSocket = _webSocket;

        if (_disposed || webSocket.IsClosed())
        {
            return false;
        }

        try
        {
            var size = 0;
            ValueWebSocketReceiveResult socketResult;

            do
            {
                if (webSocket.IsClosed())
                {
                    break;
                }

                var memory = writer.GetMemory(SocketDefaults.BufferSize);
                socketResult = await webSocket.ReceiveAsync(memory, cancellationToken);
                writer.Advance(socketResult.Count);
                size += socketResult.Count;
            } while (!socketResult.EndOfMessage);

            return size > 0;
        }
        catch
        {
            // swallow exception, there's nothing we can reasonably do.
            return false;
        }
    }

    public async ValueTask CloseAsync(
       string message,
       ConnectionCloseReason reason,
       CancellationToken cancellationToken = default)
    {
        try
        {
            var webSocket = _webSocket;

            if (_disposed || webSocket.IsClosed())
            {
                return;
            }

            await webSocket.CloseAsync(
                MapCloseStatus(reason),
                message,
                cancellationToken);

            Dispose();
        }
        catch
        {
            // we do not throw here ...
        }
    }

    public async ValueTask CloseAsync(
        string message,
        int reason,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var webSocket = _webSocket;

            if (_disposed || webSocket.IsClosed())
            {
                return;
            }

            await webSocket.CloseAsync(
                (WebSocketCloseStatus)reason,
                message,
                cancellationToken);

            Dispose();
        }
        catch
        {
            // we do not throw here ...
        }
    }

    private static WebSocketCloseStatus MapCloseStatus(ConnectionCloseReason closeReason)
        => closeReason switch
        {
            ConnectionCloseReason.EndpointUnavailable => WebSocketCloseStatus.EndpointUnavailable,
            ConnectionCloseReason.InternalServerError => WebSocketCloseStatus.InternalServerError,
            ConnectionCloseReason.InvalidMessageType => WebSocketCloseStatus.InvalidMessageType,
            ConnectionCloseReason.InvalidPayloadData => WebSocketCloseStatus.InvalidPayloadData,
            ConnectionCloseReason.MandatoryExtension => WebSocketCloseStatus.MandatoryExtension,
            ConnectionCloseReason.MessageTooBig => WebSocketCloseStatus.MessageTooBig,
            ConnectionCloseReason.NormalClosure => WebSocketCloseStatus.NormalClosure,
            ConnectionCloseReason.PolicyViolation => WebSocketCloseStatus.PolicyViolation,
            ConnectionCloseReason.ProtocolError => WebSocketCloseStatus.ProtocolError,
            _ => WebSocketCloseStatus.Empty
        };

    public void Dispose()
    {
        if (!_disposed)
        {
            _webSocket?.Dispose();
            _webSocket = null;
            _disposed = true;
        }
    }
}
