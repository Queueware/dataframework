using LinqKit;
using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

public partial class MockDataSource
{
    public void Remove<TId, TEntity>(TEntity entityForRemoval) where TEntity : class, IId<TId>
    {
        Remove<TId, TEntity>([entityForRemoval]);
    }

    public void Remove<TId, TEntity>(IEnumerable<TEntity> entitiesForRemoval)
        where TEntity : class, IId<TId>
    {
        (entitiesForRemoval ?? []).ForEach(entity => UpdateSource<TId, TEntity>(entity, SourceModificationType.Delete));
    }
}
