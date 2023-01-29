using ProjectX.Core.Events;
using ProjectX.RabbitMq.Publisher;

namespace ProjectX.Core.Realtime.Transaction;

public sealed class TransactionCommitedRealtimeHandler : IApplicationEventHandler<TransactionCommitedEvent>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IRealtimeTransactionContext _transactionContext;

    public TransactionCommitedRealtimeHandler(IMessageBroker messageBroker, IRealtimeTransactionContext transactionContext)
    {
        _messageBroker = messageBroker;
        _transactionContext = transactionContext;
    }

    public Task Handle(TransactionCommitedEvent @event, CancellationToken cancellationToken)
    {
        foreach (var message in _transactionContext.ExtractMessages())
        {
            _messageBroker.Publish(message.Item1, message.Item2);
        }

        return Task.CompletedTask;
    }
}
