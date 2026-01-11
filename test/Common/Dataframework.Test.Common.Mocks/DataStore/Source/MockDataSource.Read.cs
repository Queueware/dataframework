using Force.DeepCloner;
using LinqKit;
using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

public partial class MockDataSource
{
    public TEntity? FirstOrDefault<TId, TEntity>(Func<TEntity, bool> func) where TEntity : class, IId<TId>
    {
        return GetEnumeratedEntitiesAs<TId, TEntity>().FirstOrDefault(func)?.DeepClone();
    }

    public TEntity? Get<TId, TEntity>(TId id) where TEntity : class, IId<TId>
    {
        return Get<TId, TEntity>([id]).FirstOrDefault();
    }

    public IEnumerable<TEntity> Get<TId, TEntity>(IEnumerable<TId> ids) where TEntity : class, IId<TId>
    {
        List<TEntity> entities = [];
        var entitySet = GetInitializedEntitySet<TEntity>();
        ids.Where(id => id != null && !id.Equals(default)).ForEach(id =>
        {
            if (entitySet.TryGetValue(id!, out var value))
            {
                var retrievedEntity = ((TEntity?)value)?.DeepClone();
                if (retrievedEntity != null)
                {
                    entities.Add(retrievedEntity);
                }
            }
        });

        return entities;
    }

    public IEnumerable<TEntity> Get<TId, TEntity>() where TEntity : class, IId<TId>
    {
        return GetEnumeratedEntitiesAs<TId, TEntity>().Select(entity => entity.DeepClone());
    }

    public IEnumerable<TEntity> Where<TId, TEntity>(Func<TEntity, bool> func) where TEntity : class, IId<TId>
    {
        return GetEnumeratedEntitiesAs<TId, TEntity>().Where(func);
    }

    private IEnumerable<TEntity> GetEnumeratedEntitiesAs<TId, TEntity>() where TEntity : class, IId<TId>
    {
        return GetInitializedEntitySet<TEntity>()
            .Values.Select(dynamicEntity => dynamicEntity as TEntity)
            .OfType<TEntity>() ?? [];
    }

    private IDictionary<object, dynamic>? GetEntitySet<TEntity>()
    {
        return _storage.ContainsKey(typeof(TEntity)) ? _storage[typeof(TEntity)] : null;
    }

    private IDictionary<object, dynamic> GetInitializedEntitySet<TEntity>()
    {
        var entitySet = GetEntitySet<TEntity>();
        if (entitySet == null)
        {
            _storage.Add(typeof(TEntity), new Dictionary<object, dynamic>());
            entitySet = GetEntitySet<TEntity>();
        }

        return entitySet!;
    }
}
