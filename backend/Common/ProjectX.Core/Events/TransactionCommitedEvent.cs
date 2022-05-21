namespace ProjectX.Core;

public sealed class TransactionCommitedEvent : INotification
{
}

public interface ITransactionCommitedEventHandler : INotificationHandler<TransactionCommitedEvent>
{
}