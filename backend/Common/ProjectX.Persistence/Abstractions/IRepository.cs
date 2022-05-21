﻿using AutoMapper;
using System.Linq.Expressions;

namespace ProjectX.Persistence;

public interface IRepository<TEntity>
    where TEntity : class
{
    #region Methods with automapper
    
    Task<TOut[]> GetAsync<TOut>(IMapper mapper, IOrderingOptions ordering = null, CancellationToken cancellationToken = default);
    Task<TOut[]> GetAsync<TOut>(Expression<Func<TEntity, bool>> expression, IMapper mapper, IOrderingOptions ordering = null, CancellationToken cancellationToken = default);
    Task<PaginatedResultOf<TOut[]>> GetAsync<TOut>(IMapper mapper, IPaginationOptions pagination, IOrderingOptions ordering = null, CancellationToken cancellationToken = default);
    Task<PaginatedResultOf<TOut[]>> GetAsync<TOut>(Expression<Func<TEntity, bool>> expression, IMapper mapper, IPaginationOptions pagination, IOrderingOptions ordering = null, CancellationToken cancellationToken = default);

    Task<ResultOf<TOut>> FirstOrDefaultAsync<TOut>(Expression<Func<TEntity, bool>> expression, IMapper mapper, CancellationToken cancellationToken = default) where TOut : class;
    
    #endregion

    #region Get
    
    Task<ResultOf<TEntity>> FirstOrDefaultAsync(CancellationToken cancellationToken = default);
    Task<ResultOf<TEntity>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

    Task<TEntity[]> GetAsync(IOrderingOptions ordering = null, CancellationToken cancellationToken = default);
    Task<TEntity[]> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<TEntity[]> GetAsync(Expression<Func<TEntity, bool>> expression, IOrderingOptions ordering = null, CancellationToken cancellationToken = default);
    Task<PaginatedResultOf<TEntity[]>> GetAsync(Expression<Func<TEntity, bool>> expression, IPaginationOptions pagination, CancellationToken cancellationToken = default);
    Task<PaginatedResultOf<TEntity[]>> GetAsync(Expression<Func<TEntity, bool>> expression, IPaginationOptions pagination, IOrderingOptions ordering = null, CancellationToken cancellationToken = default);

    Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    
    #endregion

    #region Insert
    
    ValueTask InsertAsync(TEntity item, CancellationToken cancellationToken = default);
    Task InsertRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);
    
    #endregion

    #region Update
    
    void Update(TEntity item);
    void UpdateRange(IEnumerable<TEntity> items);
    void AttachRange(IEnumerable<TEntity> items);
    void Attach(TEntity item);
    
    #endregion

    #region Remove
    
    bool Remove(TEntity item);
    Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    void RemoveRange(IEnumerable<TEntity> items);
    Task RemoveRangeAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    
    #endregion

    IUnitOfWork UnitOfWork { get; }
}
