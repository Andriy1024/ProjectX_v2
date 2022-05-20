namespace ProjectX.Core;

public interface ICommand : ICommand<Unit> {}

public interface ICommand<TResult> : IRequest<Response<TResult>> {}

public interface IQuery<TResult> : IRequest<Response<TResult>> {}


public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand {}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Response<TResponse>>
    where TCommand : ICommand<TResponse> {}

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Response<TResponse>>
    where TQuery : IQuery<TResponse> {}