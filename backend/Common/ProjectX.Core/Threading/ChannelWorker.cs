﻿using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace ProjectX.Core.Threading;

public class ChannelWorker<T> : IDisposable
{
    private readonly ChannelWriter<T> _writer;
    private readonly ChannelReader<T> _reader;
    private readonly ILogger<ChannelWorker<T>> _logger;
    private readonly Func<T, Task> _handler;
    private bool _isDisposed;

    public ChannelWorker(Func<T, Task> handler, ILogger<ChannelWorker<T>> logger)
    {
        var channel = Channel.CreateUnbounded<T>();
        _reader = channel.Reader;
        _writer = channel.Writer;
        _logger = logger;
        _handler = handler;

        Task.Factory.StartNew(ReadChannelAsync, TaskCreationOptions.LongRunning);
    }

    private async Task ReadChannelAsync()
    {
        while (await _reader.WaitToReadAsync())
        {
            var job = await _reader.ReadAsync();

            try
            {
                await _handler(job);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}, {e.InnerException}, {e.StackTrace}");
            }
        }
    }

    public async ValueTask EnqueueAsync(T job)
    {
        if (_isDisposed)
        {
            _logger.LogWarning("Trying write to disposed channel.");

            return;
        }

        await _writer.WriteAsync(job);
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _writer.Complete();
        _isDisposed = true;
    }
}
