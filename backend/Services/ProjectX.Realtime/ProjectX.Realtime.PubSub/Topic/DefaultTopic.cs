﻿using ProjectX.Realtime.PubSub.SourceStream;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Channels;

using static System.Runtime.InteropServices.CollectionsMarshal;
using static System.Threading.Channels.Channel;

namespace ProjectX.Realtime.PubSub.Topic;

/// <summary>
/// This base class can be used to implement a subscription provider and already
/// implements a lot of the logic needed for the typical pub/sub topic like backpressure/throttling.
/// </summary>
/// <typeparam name="TMessage">
/// The message.
/// </typeparam>
public abstract class DefaultTopic<TMessage> : ITopic
{
    private readonly CancellationTokenSource _cts = new();
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly Channel<TMessage> _incoming;
    private readonly BoundedChannelOptions _channelOptions;
    private readonly List<Channel<TMessage>> _subscribers = new();
    private bool _completed;
    private bool _disposed;

    public event EventHandler<EventArgs>? Closed;

    protected DefaultTopic(
        string name,
        int capacity,
        TopicBufferFullMode fullMode)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _channelOptions = new BoundedChannelOptions(capacity)
        {
            FullMode = (BoundedChannelFullMode)(int)fullMode
        };
        _incoming = CreateUnbounded<TMessage>();
    }

    protected DefaultTopic(
        string name,
        int capacity,
        TopicBufferFullMode fullMode,
        Channel<TMessage> incomingMessages)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _channelOptions = new BoundedChannelOptions(capacity)
        {
            FullMode = (BoundedChannelFullMode)(int)fullMode
        };
        _incoming = incomingMessages;
    }

    /// <summary>
    /// Gets the name of this topic.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the message type of this topic.
    /// </summary>
    public Type MessageType => typeof(TMessage);

    /// <summary>
    /// Publishes a new message to this topic.
    /// </summary>
    /// <param name="message">
    /// The message that shall be published.
    /// </param>
    public void Publish(TMessage message)
        => _incoming.Writer.TryWrite(message);

    /// <summary>
    /// Completes this topic.
    /// </summary>
    public void Complete()
    {
        if (_completed)
        {
            return;
        }

        _lock.EnterWriteLock();

        try
        {
            _incoming.Writer.TryComplete();
            _completed = true;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Allows to subscribe to this topic. If the topic is already completed, this method will
    /// return null.
    /// </summary>
    /// <returns>
    /// Returns a source stream that allows to read messages from this topic.
    /// </returns>
    internal ISourceStream<TMessage>? TrySubscribe()
    {
        _lock.EnterReadLock();

        try
        {
            if (_completed)
            {
                // it could have happened that we have entered subscribe after it was
                // already be completed. In this case we will return null to
                // signal that subscribe was unsuccessful.
                return null;
            }

            var stream = SubscribeUnsafe();

            return stream;
        }
        catch
        {
            // it could have happened that we entered subscribe at the moment dispose is hit,
            // in this case we return null to signal that subscribe was unsuccessful.
            return null;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    private DefaultSourceStream<TMessage> SubscribeUnsafe()
    {
        var channel = CreateBounded<TMessage>(_channelOptions);
        var stream = new DefaultSourceStream<TMessage>(this, channel);
        _subscribers.Add(channel);
        return stream;
    }

    /// <summary>
    /// Allows the subscriber to signal that it is no longer interested in the topic.
    /// </summary>
    /// <param name="channel">
    /// The subscribing channel.
    /// </param>
    internal void Unsubscribe(Channel<TMessage> channel)
    {
        _lock.EnterWriteLock();

        try
        {
            _subscribers.Remove(channel);


            if (_subscribers.Count == 0)
            {
                CloseUnsafe();
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    internal async ValueTask ConnectAsync(CancellationToken ct = default)
    {
        try
        {
            var session = await OnConnectAsync(ct).ConfigureAwait(false);

            _ = BeginProcessing(session);
        }
        catch (Exception ex)
        {
            //TODO: log error
        }
    }

    private Task BeginProcessing(IDisposable session)
        => Task.Factory.StartNew(
            async s => await ProcessMessagesSessionAsync((IDisposable)s!).ConfigureAwait(false),
            session);

    private async Task ProcessMessagesSessionAsync(IDisposable session)
    {
        try
        {
            await ProcessMessagesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            //TODO: log error
        }
        finally
        {
            session.Dispose();
        }
    }

    private async Task ProcessMessagesAsync()
    {
        var ct = _cts.Token;

        try
        {
            while (await _incoming.Reader.WaitToReadAsync(ct).ConfigureAwait(false))
            {
                DispatchMessages();
            }
        }
        finally
        {
            Dispose();
        }
    }

    private void DispatchMessages()
    {
        _lock.EnterReadLock();

        try
        {
            var iterations = 0;

            while (_incoming.Reader.TryRead(out var message))
            {
                var subscribersSpan = AsSpan(_subscribers);
                ref var start = ref MemoryMarshal.GetReference(subscribersSpan);
                ref var end = ref Unsafe.Add(ref start, subscribersSpan.Length);

                var allWritesSuccessful = true;

                while (Unsafe.IsAddressLessThan(ref start, ref end))
                {
                    if (!start.Writer.TryWrite(message))
                    {
                        allWritesSuccessful = false;
                    }
                    start = ref Unsafe.Add(ref start, 1);
                }

                if (!allWritesSuccessful || iterations++ >= 8)
                {
                    // we will take a pause if we have dispatched 8 messages or if we could not dispatch all messages.
                    // This will give time for subscribers to unsubscribe and
                    // others to hop on.
                    break;
                }
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    protected void Close()
    {
        if (_disposed)
        {
            return;
        }

        _lock.EnterWriteLock();

        try
        {
            CloseUnsafe();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private void CloseUnsafe()
    {
        if (_disposed)
        {
            return;
        }

        _incoming.Writer.TryComplete();

        try
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Writer.TryComplete();
            }

            _subscribers.Clear();
        }
        finally
        {
            _completed = true;
            _disposed = true;
            RaiseClosedEvent();
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    /// <summary>
    /// Override this method to connect to an external pub/sub provider.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// Returns a session to dispose the subscription session.
    /// </returns>
    protected virtual ValueTask<IDisposable> OnConnectAsync(
        CancellationToken cancellationToken)
        => new(DefaultSession.Instance);

    /// <summary>
    /// Signal that this topic was closed.
    /// </summary>
    private void RaiseClosedEvent()
        => Closed?.Invoke(this, EventArgs.Empty);

    ~DefaultTopic()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Close();
            }

            _completed = true;
            _disposed = true;
        }
    }

    private sealed class DefaultSession : IDisposable
    {
        private DefaultSession() { }

        public void Dispose() { }

        public static readonly DefaultSession Instance = new();
    }
}