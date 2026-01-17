using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

public partial class MockDataSource
{
    public void Add<TId, TEntity>(TEntity entity) where TEntity : class, IId<TId>
    {
        if (entity != null!)
        {
            Add<TId, TEntity>([entity]);
        }
    }

    public void Add<TId, TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IId<TId>
    {
        ModifySource<TId, TEntity>(GetInitializedEntitySet<TEntity>(), entities, SourceModificationType.Create);
    }
}
