﻿using ProjectX.Core.Results;

namespace ProjectX.Core.Validation;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IValidatable validatableRequest)
        {
            var failures = validatableRequest.Validate();

            if (failures.Any())
            {
                var error = ApplicationError.InvalidData(message: failures.BuildErrorMessage());

                return Task.FromResult(ResultActivator.From<TResponse>(error));
            }
        }

        return next();
    }
}