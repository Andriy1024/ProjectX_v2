using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectX.FileStorage.Database.Abstractions;
using System.Linq.Expressions;

namespace ProjectX.FileStorage.Database;

internal class MongoRepository<TEntity, TIdentifiable> : IMongoRepository<TEntity, TIdentifiable>
    where TEntity : IDocumentEntry<TIdentifiable>
	where TIdentifiable : notnull
{
	public IMongoCollection<TEntity> Collection { get; }

	public MongoRepository(IMongoDatabase database)
	{
        Collection = database.GetCollection<TEntity>(TEntity.Collection);
	}

    public Task<TEntity> GetAsync(TIdentifiable id, CancellationToken cancellationToken = default)
        => GetAsync(e => e.Id.Equals(id), cancellationToken);

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		=> Collection.Find(predicate).SingleOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.AsQueryable().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		=> await Collection.Find(predicate).ToListAsync(cancellationToken);

	public Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate, TQuery query, CancellationToken cancellationToken = default) 
		where TQuery : IPagedQuery
		=> Collection.AsQueryable().Where(predicate).PaginateAsync(query);

	public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
		=> Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

	public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
		=> UpdateAsync(entity, e => e.Id.Equals(entity.Id), cancellationToken: cancellationToken);

	public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		=> Collection.ReplaceOneAsync(predicate, entity, cancellationToken: cancellationToken);

	public Task DeleteAsync(TIdentifiable id, CancellationToken cancellationToken = default)
		=> DeleteAsync(e => e.Id.Equals(id), cancellationToken);

	public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		=> Collection.DeleteOneAsync(predicate, cancellationToken: cancellationToken);

	public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		=> Collection.Find(predicate).AnyAsync(cancellationToken);
}