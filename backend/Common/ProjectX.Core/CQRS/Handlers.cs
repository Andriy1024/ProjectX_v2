namespace ProjectX.Core;

public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public abstract Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);

    protected virtual Response<TResponse> Failed(Error error) => ResponseFactory.Failed<TResponse>(error);
    
    protected virtual Response<TResponse> Success(TResponse result) => ResponseFactory.Success(result);
}

public abstract class QueryHandler<TCommand, TResponse> : IQueryHandler<TCommand, TResponse>
    where TCommand : IQuery<TResponse>
{
    public abstract Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);

    protected virtual Response<TResponse> Failed(Error error) => ResponseFactory.Failed<TResponse>(error);

    protected virtual Response<TResponse> Success(TResponse result) => ResponseFactory.Success(result);
}