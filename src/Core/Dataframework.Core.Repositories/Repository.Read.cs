using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Core.Repositories;

public partial class Repository<TId, TEntity, TDataContext>
{
    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        using var dataContext = _dataContext();
        return await dataContext.Set<TEntity>().CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        using var dataContext = _dataContext();
        return await dataContext.Set<TEntity>().CountAsync(expression, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        using var dataContext = _dataContext();
        return await dataContext.Set<TEntity>().FindAsync(expression, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        using var dataContext = _dataContext();
        return await dataContext.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        Expression<Func<TEntity, bool>> expression = entity => entity.Id != null && entity.Id.Equals(id);
        return await FirstOrDefaultAsync(expression, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetByIdAsync(IEnumerable<TId> ids, CancellationToken cancellationToken)
    {
        return await FindAsync(entity => ids.Contains(entity.Id), cancellationToken);
    }

    private static async Task<IEnumerable<TId>> FindExistingIdsAsync(IDataContext dataContext, IEnumerable<TEntity> entities,
        CancellationToken cancellationToken)
    {
        var dataSet = dataContext.Set<TEntity>();
        var ids = entities.Select<TEntity, TId>(entity => entity.Id);
        
        return (await dataSet.FindAsync(entity => ids.Contains(entity.Id), cancellationToken))
            .Select<TEntity, TId>(entity => entity.Id)
            .ToList();
    }
}