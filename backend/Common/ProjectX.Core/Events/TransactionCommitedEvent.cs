namespace ProjectX.Core;

public class TransactionCommitedEvent : IDomainEvent
{
}

public interface ITransactionCommitedEventHandler : INotificationHandler<TransactionCommitedEvent>
{
}