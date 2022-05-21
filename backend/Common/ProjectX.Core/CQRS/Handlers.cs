namespace ProjectX.Core;

public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public abstract Task<ResultOf<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);

    protected virtual ResultOf<TResponse> Failed(Error error) => ResultFactory.Failed<TResponse>(error);
    
    protected virtual ResultOf<TResponse> Success(TResponse result) => ResultFactory.Success(result);
}

public abstract class QueryHandler<TCommand, TResponse> : IQueryHandler<TCommand, TResponse>
    where TCommand : IQuery<TResponse>
{
    public abstract Task<ResultOf<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);

    protected virtual ResultOf<TResponse> Failed(Error error) => ResultFactory.Failed<TResponse>(error);

    protected virtual ResultOf<TResponse> Success(TResponse result) => ResultFactory.Success(result);
}