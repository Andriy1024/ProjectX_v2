namespace ProjectX.Core.Realtime.Transaction;

public sealed class TransactionCommitedRealtimeHandler : ITransactionCommitedEventHandler
{
    private readonly IRabbitMqPublisher _publisher;
    private readonly IRealtimeTransactionContext _transactionContext;

    public TransactionCommitedRealtimeHandler(IRabbitMqPublisher publisher,
                                              IRealtimeTransactionContext transactionContext)
    {
        _publisher = publisher;
        _transactionContext = transactionContext;
    }

    public async Task Handle(TransactionCommitedEvent @event, CancellationToken cancellationToken)
    {
        foreach (var message in _transactionContext.ExtractMessages())
        {
            _publisher.Publish(message.Item1, message.Item2);
        }
    }
}
