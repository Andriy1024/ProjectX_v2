namespace ProjectX.Core;

public interface ICommand : ICommand<Unit> {}

public interface ICommand<TResult> : IRequest<ResultOf<TResult>> {}

public interface IQuery<TResult> : IRequest<ResultOf<TResult>> {}


public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand {}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ResultOf<TResponse>>
    where TCommand : ICommand<TResponse> {}

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, ResultOf<TResponse>>
    where TQuery : IQuery<TResponse> {}