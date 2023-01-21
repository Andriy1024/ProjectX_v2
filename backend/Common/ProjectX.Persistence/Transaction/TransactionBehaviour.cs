using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectX.Core.Abstractions;

namespace ProjectX.Persistence.Transaction;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly ILogger<TransactionBehaviour<TRequest, TResponse>> Logger;
    protected readonly IUnitOfWork UnitOfWork;

    protected bool Success { get; private set; }

    public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, IUnitOfWork unitOfWork)
    {
        Logger = logger;
        UnitOfWork = unitOfWork;
    }

    public virtual async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IHasTransaction || UnitOfWork.HasActiveTransaction)
        {
            return await next();
        }

        var response = default(TResponse);

        await UnitOfWork.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            using (var transaction = await UnitOfWork.BeginTransactionAsync())
            {
                Logger.LogInformation("----- Begin transaction {TransactionId} for ({@Command})", transaction.TransactionId, request);

                response = await next();

                Success = !(response is IResult r && r.IsFailed);

                if (Success)
                {
                    await UnitOfWork.CommitTransactionAsync(transaction);

                    Logger.LogInformation($"----- Transaction {transaction.TransactionId} was commited.");
                }
                else
                {
                    await UnitOfWork.RollbackTransactionAsync();

                    Logger.LogInformation($"----- Transaction {transaction.TransactionId} was failed.");
                }
            }
        });

        return response;
    }
}
