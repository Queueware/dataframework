using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Infrastructure.Repositories;

public partial class Repository<TId, TEntity, TDataContext>
{
    /// <inheritdoc />
    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await UpdateAsync([entity], cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var areEntitiesUpdated = false;

        var enumeratedEntities = entities.ToList();
        if (enumeratedEntities.Count > 0)
        {
            using var dataContext = _dataContext();
            areEntitiesUpdated = await UpdateAsync(dataContext, enumeratedEntities, cancellationToken);
        }

        return areEntitiesUpdated;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value, CancellationToken cancellationToken)
    {
        var isEntityUpdated = false;
        
        using var dataContext = _dataContext();
        var dataSet = dataContext.Set<TEntity>();
        var isExistingEntity = await dataSet.FirstOrDefaultAsync(existingEntity =>
            existingEntity.Id != null && existingEntity.Id.Equals(entity.Id), cancellationToken) != null;
        
        if (isExistingEntity)
        {
            await dataSet.UpdateAsync(entity, field, value, cancellationToken);
            isEntityUpdated = await dataContext.SaveChangesAsync(cancellationToken) > 0;
        }
        
        return isEntityUpdated;
    }

    private static async Task<bool> UpdateAsync(IDataContext dataContext, List<TEntity> entities,
        CancellationToken cancellationToken)
    {
        var areEntitiesUpdated = false;
        
        var existingIds = await FindExistingIdsAsync(dataContext, entities, cancellationToken);
        var existingEntitiesToUpdate = entities.Where(entity => existingIds.Contains(entity.Id)).ToList();

        if (existingEntitiesToUpdate.Count > 0)
        {
            await dataContext.Set<TEntity>().UpdateAsync(existingEntitiesToUpdate, cancellationToken);
            areEntitiesUpdated = await dataContext.SaveChangesAsync(cancellationToken) > 0;
        }
        
        return areEntitiesUpdated;
    }
}