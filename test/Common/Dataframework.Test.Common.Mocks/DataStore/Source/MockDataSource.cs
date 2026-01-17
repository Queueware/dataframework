using Force.DeepCloner;
using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

public partial class MockDataSource
{
    private readonly IDictionary<Type, IDictionary<object, dynamic>> _storage = new Dictionary<Type, IDictionary<object, dynamic>>();

    public IEnumerable<Type> StoredTypes => _storage.Keys;

    private static void ModifySource<TId, TEntity>(IDictionary<object, dynamic> entitySet, TEntity entity, SourceModificationType modificationType) where TEntity : class, IId<TId>
    {
        ModifySource<TId, TEntity>(entitySet, [entity], modificationType);
    }

    private static void ModifySource<TId, TEntity>(IDictionary<object, dynamic> entitySet, IEnumerable<TEntity> entities, SourceModificationType modificationType) where TEntity : class, IId<TId>
    {
        var entitiesForModification = (entities ?? []).Where(entity => entity != null! && entity.Id != null && !entity.Equals(default)).ToList();
        entitiesForModification.ForEach(entity => ModifySourceFrom<TId, TEntity>(entitySet, entity, modificationType));
    }

    private static void ModifySourceFrom<TId, TEntity>(IDictionary<object, dynamic> entitySet, TEntity entity, SourceModificationType modificationType) where TEntity : class, IId<TId>
    {
        var id = entity.Id;
        switch (modificationType)
        {
            case SourceModificationType.Create:
            case SourceModificationType.Update when entitySet.ContainsKey(id!):
                entitySet[id!] = entity.DeepClone();
                break;
            case SourceModificationType.Delete:
            {
                if (entitySet.ContainsKey(id!))
                {
                    entitySet.Remove(id!);
                }

                break;
            }
            case SourceModificationType.Unspecified:
            default:
                break;
        }
    }

    private enum SourceModificationType
    {
        Unspecified = 0,
        Create = 1,
        Delete = 2,
        Update = 3
    }
}