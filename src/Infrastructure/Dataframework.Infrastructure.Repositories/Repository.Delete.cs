using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Infrastructure.Repositories;

public partial class Repository<TId, TEntity, TDataContext>
{
    /// <inheritdoc />
    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await DeleteAsync([entity], cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var ids = entities.Select(entity => entity.Id);
        return DeleteAsync(ids, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        return DeleteAsync([id], cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken)
    {
        var areEntitiesDeleted = false;

        var enumeratedIds = ids.ToList();
        if (enumeratedIds.Count > 0)
        {
            using var dataContext = _dataContext();
            areEntitiesDeleted = await DeleteAsync(dataContext, enumeratedIds, cancellationToken);
        }

        return areEntitiesDeleted;
    }

    private static async Task<bool> DeleteAsync(IDataContext dataContext, IEnumerable<TId> ids,
        CancellationToken cancellationToken)
    {
        var dataSet = dataContext.Set<TEntity>();
        var entitiesToDelete = (await dataSet
            .FindAsync(entity => ids.Contains(entity.Id), cancellationToken)).ToList();

        return await DeleteAsync(dataContext, dataSet, entitiesToDelete, cancellationToken);
    }

    private static async Task<bool> DeleteAsync(IDataContext dataContext, IDataSet<TEntity> dataSet,
        List<TEntity> entities, CancellationToken cancellationToken)
    {
        var areEntitiesDeleted = false;
        
        if (entities.Count > 0)
        {
            await dataSet.RemoveAsync(entities, cancellationToken);
            areEntitiesDeleted = await dataContext.SaveChangesAsync(cancellationToken) > 0;
        }
        
        return areEntitiesDeleted;
    }
}