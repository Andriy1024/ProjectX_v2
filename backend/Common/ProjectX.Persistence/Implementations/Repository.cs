﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectX.Persistence.Extensions;
using System.Linq.Expressions;

namespace ProjectX.Persistence.Implementations;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class//, IEntity
{
    protected DbSet<TEntity> DbSet { get; }

    protected ErrorCode NotFound { get; private set; }

    public IUnitOfWork UnitOfWork { get; }

    public Repository(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        DbSet = UnitOfWork.DbContext.Set<TEntity>();
        SetNotFoundCode(ErrorCode.NotFound);
    }

    #region Select

    public virtual Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(expression, cancellationToken);
    }

    public virtual async Task<ResultOf<TEntity>> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return GetResultOf(await DbSet.FirstOrDefaultAsync(cancellationToken));
    }

    public virtual async Task<ResultOf<TEntity>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return GetResultOf(await DbSet.FirstOrDefaultAsync(expression, cancellationToken));
    }

    public virtual Task<TEntity[]> GetAsync(IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        return query.ToArrayAsync(cancellationToken);
    }

    public virtual Task<TEntity[]> GetAsync(Expression<Func<TEntity, bool>> expression, IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Where(expression);

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        return query.ToArrayAsync(cancellationToken);
    }

    public virtual Task<TEntity[]> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return DbSet.AsNoTracking().Where(expression).ToArrayAsync(cancellationToken);
    }

    public virtual async Task<PaginatedResultOf<TEntity[]>> GetAsync(Expression<Func<TEntity, bool>> expression, IPaginationOptions pagination, CancellationToken cancellationToken)
    {
        var entities = await DbSet.Where(expression)
                                  .WithPagination(pagination)
                                  .ToArrayAsync(cancellationToken);

        var total = await CountAsync(expression, entities, pagination, cancellationToken);
        
        return (entities, total);
    }

    public virtual async Task<PaginatedResultOf<TEntity[]>> GetAsync(Expression<Func<TEntity, bool>> expression, IPaginationOptions pagination, IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Where(expression);

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        var entities = await query.WithPagination(pagination).ToArrayAsync(cancellationToken);

        var total = await CountAsync(expression, entities, pagination, cancellationToken);

        return (entities, total);
    }

    #endregion

    #region Insert

    public virtual async ValueTask InsertAsync(TEntity item, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(item, cancellationToken);
    }

    public virtual Task InsertRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
    {
        return DbSet.AddRangeAsync(items, cancellationToken);
    }

    #endregion

    #region Update

    public virtual void AttachRange(IEnumerable<TEntity> items)
    {
        DbSet.AttachRange(items);
    }

    public virtual void Attach(TEntity item)
    {
        DbSet.Attach(item);
    }

    public virtual void Update(TEntity item)
    {
        DbSet.Update(item);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> items)
    {
        DbSet.UpdateRange(items);
    }

    #endregion

    #region Remove

    public virtual bool Remove(TEntity item) => DbSet.Remove(item).State == EntityState.Deleted;

    public virtual async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FirstOrDefaultAsync(expression, cancellationToken);

        return entity != null && DbSet.Remove(entity).State == EntityState.Deleted;
    }

    public virtual void RemoveRange(IEnumerable<TEntity> items)
    {
        DbSet.RemoveRange(items);
    }

    public virtual async Task RemoveRangeAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        var entities = await DbSet.Where(expression).ToArrayAsync(cancellationToken);

        if (entities.Length == 0)
            return;

        DbSet.RemoveRange(entities);
    }

    #endregion

    #region Methods with automapper

    public virtual async Task<ResultOf<TOut>> FirstOrDefaultAsync<TOut>(Expression<Func<TEntity, bool>> expression, IMapper mapper, CancellationToken cancellationToken = default)
        where TOut : class
    {
        var result = await DbSet.AsNoTracking()
                                .Where(expression)
                                .ProjectTo<TOut>(mapper.ConfigurationProvider)
                                .FirstOrDefaultAsync(cancellationToken);

        return GetResultOf(result);
    }

    public virtual Task<TOut[]> GetAsync<TOut>(IMapper mapper, IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        return query.ProjectTo<TOut>(mapper.ConfigurationProvider).ToArrayAsync(cancellationToken);
    }

    public virtual Task<TOut[]> GetAsync<TOut>(Expression<Func<TEntity, bool>> expression, IMapper mapper, IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().Where(expression);

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        return query.ProjectTo<TOut>(mapper.ConfigurationProvider).ToArrayAsync(cancellationToken);
    }

    public async virtual Task<PaginatedResultOf<TOut[]>> GetAsync<TOut>(IMapper mapper, IPaginationOptions pagination, IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        var entities = await query.WithPagination(pagination)
                                  .ProjectTo<TOut>(mapper.ConfigurationProvider)
                                  .ToArrayAsync(cancellationToken);

        var total = await CountAsync(entities, pagination, cancellationToken);
        
        return (entities, total);
    }

    public async virtual Task<PaginatedResultOf<TOut[]>> GetAsync<TOut>(Expression<Func<TEntity, bool>> expression, IMapper mapper, IPaginationOptions pagination, IOrderingOptions ordering = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().Where(expression);

        if (ordering != null)
        {
            query = query.WithOrdering(ordering);
        }

        var entities = await query.WithPagination(pagination)
                                  .ProjectTo<TOut>(mapper.ConfigurationProvider)
                                  .ToArrayAsync(cancellationToken);

        var total = await CountAsync(expression, entities, pagination, cancellationToken);

        return (entities, total);
    }

    #endregion

    #region Inner methods

    protected void SetNotFoundCode(ErrorCode code) 
    {
        NotFound = code;
    }

    protected ResultOf<TOut> GetResultOf<TOut>(TOut? entity) where TOut : class
    {
        return entity == null
            ? Error.NotFound(NotFound, $"{typeof(TOut).Name} not found.")
            : entity;
    }

    protected ValueTask<int> CountAsync<T>(Expression<Func<TEntity, bool>> expression, T[] entities, IPaginationOptions pagination, CancellationToken cancellationToken)
    {
        return pagination.Skip == 0 && entities.Length < pagination.Take
                  ? new ValueTask<int>(entities.Length)
                  : new ValueTask<int>(DbSet.CountAsync(expression, cancellationToken));
    }

    protected ValueTask<int> CountAsync<T>(T[] entities, IPaginationOptions pagination, CancellationToken cancellationToken)
    {
        return pagination.Skip == 0 && entities.Length < pagination.Take
                  ? new ValueTask<int>(entities.Length)
                  : new ValueTask<int>(DbSet.CountAsync(cancellationToken));
    }

    #endregion
}