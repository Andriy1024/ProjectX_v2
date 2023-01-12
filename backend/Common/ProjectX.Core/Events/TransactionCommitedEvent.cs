using ProjectX.Core.Events;

namespace ProjectX.Core;

public class TransactionCommitedEvent : IApplicationEvent
{
}

public interface ITransactionCommitedEventHandler : IApplicationEventHandler<TransactionCommitedEvent>
{
}