namespace ProjectX.Core.Validation;

//public interface ICommandBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, ResultOf<TResponse>>
//    where TCommand : ICommand<TResponse>
//{ 
//}

//public class ValidationBehavior<TCommand, TResponse> : ICommandBehavior<TCommand, TResponse>
//    where TCommand : ICommand<TResponse>
//{
//    public async Task<ResultOf<TResponse>> Handle(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<ResultOf<TResponse>> next)
//    {
//        if(request is null) 
//        {
//            return new ResultOf<TResponse>(Error.ServerError(message: "Tests"));
//        }

//        return await next();
//    }
//}

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        //TODO: Test commented code above.
        if (request is IValidatable validatableRequest)
        {
            validatableRequest.Validate();
        }

        return next();
    }
}