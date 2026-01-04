using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Infrastructure.DataSources.Sql;

/// <summary>
/// Represents a SQL-backed implementation of <see cref="IDataSet{TEntity}"/>
/// that delegates all operations to an underlying Entity Framework Core
/// <see cref="DbSet{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">
/// The entity type managed by the data set.
/// </typeparam>
/// <param name="dbSet">
/// The underlying Entity Framework Core <see cref="DbSet{TEntity}"/> used to
/// perform query and persistence operations.
/// </param>
public class SqlDataSet<TEntity>(DbSet<TEntity> dbSet) : IDataSet<TEntity> where TEntity : class
{
    /// <summary>
    /// Exposes the underlying entity set as an <see cref="IQueryable{T}"/>
    /// to enable deferred execution and composable LINQ queries.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{T}"/> representing the entity set.
    /// </returns>
    public IQueryable<TEntity> AsQueryable() => dbSet.AsQueryable();

    /// <summary>
    /// Adds a new entity to the data set.
    /// </summary>
    /// <remarks>
    /// The entity is tracked by the owning data context. Changes are not
    /// persisted until <c>SaveChangesAsync</c> is invoked on the
    /// associated <see cref="IDataContext"/>.
    /// </remarks>
    /// <param name="entity">
    /// The entity to add.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return dbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    /// <summary>
    /// Adds multiple entities to the data set.
    /// </summary>
    /// <remarks>
    /// All entities are tracked by the owning data context. Changes are not
    /// persisted until <c>SaveChangesAsync</c> is invoked.
    /// </remarks>
    /// <param name="entities">
    /// The entities to add.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return dbSet.AddRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// Returns the number of entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="expression">
    /// A predicate used to filter the entities.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// The number of matching entities.
    /// </returns>
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return dbSet.CountAsync(expression, cancellationToken);
    }

    /// <summary>
    /// Finds all entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="expression">
    /// A predicate used to filter the entities.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// A collection of entities that match the predicate.
    /// </returns>
    public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        return Task.Run(() => dbSet.Where(expression).AsEnumerable(), cancellationToken);
    }

    /// <summary>
    /// Returns the first entity that satisfies the specified predicate,
    /// or <c>null</c> if no such entity exists.
    /// </summary>
    /// <param name="expression">
    /// A predicate used to filter the entities.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// The first matching entity, or <c>null</c>.
    /// </returns>
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        return dbSet.FirstOrDefaultAsync(expression, cancellationToken);
    }

    /// <summary>
    /// Removes the specified entity from the data set.
    /// </summary>
    /// <remarks>
    /// The entity is marked for deletion. The removal is not persisted
    /// until <c>SaveChangesAsync</c> is invoked.
    /// </remarks>
    /// <param name="entity">
    /// The entity to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => dbSet.Remove(entity), cancellationToken);
    }

    /// <summary>
    /// Removes multiple entities from the data set.
    /// </summary>
    /// <remarks>
    /// All entities are marked for deletion. The removal is not persisted
    /// until <c>SaveChangesAsync</c> is invoked.
    /// </remarks>
    /// <param name="entities">
    /// The entities to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task RemoveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.Run(() => dbSet.RemoveRange(entities), cancellationToken);
    }

    /// <summary>
    /// Updates the specified entity in the data set.
    /// </summary>
    /// <remarks>
    /// The entity must be tracked or attachable by the owning data context.
    /// Changes are not persisted until <c>SaveChangesAsync</c> is invoked.
    /// </remarks>
    /// <param name="entity">
    /// The entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => dbSet.Update(entity), cancellationToken);
    }

    /// <summary>
    /// Updates multiple entities in the data set.
    /// </summary>
    /// <remarks>
    /// All entities must be tracked or attachable by the owning data context.
    /// Changes are not persisted until <c>SaveChangesAsync</c> is invoked.
    /// </remarks>
    /// <param name="entities">
    /// The entities to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.Run(() => dbSet.UpdateRange(entities), cancellationToken);
    }

    /// <summary>
    /// Updates a single field of the specified entity with the given value.
    /// </summary>
    /// <typeparam name="TField">
    /// The type of the field being updated.
    /// </typeparam>
    /// <param name="entity">
    /// The entity to update.
    /// </param>
    /// <param name="field">
    /// An expression identifying the field to update.
    /// </param>
    /// <param name="value">
    /// The new value to assign to the field.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    public Task UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value,
        CancellationToken cancellationToken)
    {
        return Task.Run(() => dbSet.Entry(entity).Property(field).CurrentValue = value, cancellationToken);
    }
}