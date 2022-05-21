using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ProjectX.Persistence;

/// <summary>
/// UnitOfWork is used to manage DB transaction.
/// </summary>
public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);

    IExecutionStrategy CreateExecutionStrategy();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task CommitTransactionAsync(IDbContextTransaction transaction);

    Task RollbackTransactionAsync();

    IDbContextTransaction GetCurrentTransaction();

    IDbConnection GetCurrentConnection();

    bool HasActiveTransaction { get; }

    DbContext DbContext { get; }
}