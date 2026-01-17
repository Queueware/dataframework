using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Infrastructure.Repositories;

public partial class Repository<TId, TEntity, TDataContext>
{
    /// <inheritdoc />
    public async Task<bool> InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await InsertAsync([entity], cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var areEntitiesInserted = false;

        var enumeratedEntities = entities.ToList();
        if (enumeratedEntities.Count > 0)
        {
            using var dataContext = _dataContext();
            areEntitiesInserted = await InsertAsync(dataContext, enumeratedEntities, cancellationToken);
        }

        return areEntitiesInserted;
    }

    private static async Task<bool> InsertAsync(IDataContext dataContext, List<TEntity> enumeratedEntities,
        CancellationToken cancellationToken)
    {
        var existingEntityIds = await FindExistingIdsAsync(dataContext, enumeratedEntities, cancellationToken);
        var newEntities = enumeratedEntities.Where(entity => !existingEntityIds.Contains(entity.Id)).ToList();
        var dataSet = dataContext.Set<TEntity>();

        return await InsertAsync(dataContext, dataSet, newEntities, cancellationToken);
    }


    private static async Task<bool> InsertAsync(IDataContext dataContext, IDataSet<TEntity> dataSet,
        List<TEntity> entities, CancellationToken cancellationToken)
    {
        var areEntitiesInserted = false;
        
        if (entities.Count > 0)
        {
            await dataSet.AddAsync(entities, cancellationToken);
            areEntitiesInserted = await dataContext.SaveChangesAsync(cancellationToken) > 0;
        }

        return areEntitiesInserted;
    }
}