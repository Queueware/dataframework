using System.Linq.Expressions;
using System.Reflection;
using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

public partial class MockDataSource
{
    public void Update<TId, TEntity, TField>(TId id, Expression<Func<TEntity, TField?>> field, TField? value)
        where TEntity : class, IId<TId>
    {
        var entityToUpdate = Get<TId, TEntity>(id);
        Update<TId, TEntity, TField>(entityToUpdate != null ? [entityToUpdate] : [], field, value);
    }

    public void Update<TId, TEntity, TField>(IEnumerable<TId> ids, Expression<Func<TEntity, TField?>> field, TField? value)
        where TEntity : class, IId<TId>
    {
        var entitiesToUpdate = Get<TId, TEntity>(ids);
        Update<TId, TEntity, TField>(entitiesToUpdate , field, value);
    }

    public void Update<TId, TEntity>(TEntity entityForUpdate) where TEntity : class, IId<TId>
    {
        Update<TId, TEntity>([entityForUpdate]);
    }

    public void Update<TId, TEntity>(IEnumerable<TEntity> entitiesToUpdate)
        where TEntity : class, IId<TId>
    {
        (entitiesToUpdate ?? []).ToList().ForEach(entity => UpdateSource<TId, TEntity>(entity));
    }

    private void UpdateSource<TId, TEntity>(TEntity entityForModification, SourceModificationType modificationType = SourceModificationType.Update) where TEntity : class, IId<TId>
    {
        ModifySource<TId, TEntity>(GetInitializedEntitySet<TEntity>(), entityForModification, modificationType);
    }

    private void Update<TId, TEntity, TField>(IEnumerable<TEntity> entities, Expression<Func<TEntity, TField?>> field, TField? value)
        where TEntity : class, IId<TId>
    {
        if (field != null!)
        {
            var entitiesToUpdate = entities?.Where(entity => entity != null!).ToList() ?? [];
            var fieldName = ((field.Body as MemberExpression)?.Member as PropertyInfo)?.Name;
            UpdateEntityField<TId, TEntity, TField>(entitiesToUpdate, fieldName, value);
        }
    }

    private void UpdateEntityField<TId, TEntity, TField>(IEnumerable<TEntity> entities, string? fieldName, TField? value) 
        where TEntity : class, IId<TId>
    {
        var property = fieldName != null ? typeof(TEntity).GetProperty(fieldName) : null;
        if (property != null)
        {
            var entitiesToUpdate = entities.ToList();
            entitiesToUpdate.ForEach(entity => property.SetValue(entity, value));
            Update<TId, TEntity>(entitiesToUpdate);
        }
    }

    private PropertyInfo? GetEntityProperty<TEntity>(string? fieldName)
    {
        return fieldName != null ? typeof(TEntity).GetProperty(fieldName) : null;
    }

    private void SetEntityFieldValue<TEntity, TField>(PropertyInfo? property, TEntity entity, TField? value)
    {
        if (property != null)
        {
            property.SetValue(entity, value);
        }
    }
}
