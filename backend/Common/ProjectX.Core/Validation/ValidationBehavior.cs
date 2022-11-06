using ProjectX.Core.Results;

namespace ProjectX.Core.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request is IValidatable validatableRequest)
        {
            var failures = validatableRequest.Validate();

            if (failures.Any())
            {
                var error = Error.InvalidData(message: failures.BuildErrorMessage());

                return Task.FromResult(ResultActivator.From<TResponse>(error));
            }
        }

        return next();
    }
}