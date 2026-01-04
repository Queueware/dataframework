using System.Linq.Expressions;

namespace Queueware.Dataframework.Abstractions.DataSources;

/// <summary>
/// Represents a generic data set abstraction for querying and persisting
/// entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// The entity type managed by the data set.
/// </typeparam>
public interface IDataSet<TEntity> where TEntity : class
{
    /// <summary>
    /// Exposes the underlying data source as an <see cref="IQueryable{T}"/>
    /// to enable deferred execution and composable queries.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{T}"/> representing the entity set.
    /// </returns>
    IQueryable<TEntity> AsQueryable();

    /// <summary>
    /// Adds a new entity to the data set.
    /// </summary>
    /// <param name="entity">
    /// The entity to add.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Adds multiple entities to the data set.
    /// </summary>
    /// <param name="entities">
    /// The entities to add.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

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
    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

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
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken);

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
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

    /// <summary>
    /// Removes the specified entity from the data set.
    /// </summary>
    /// <param name="entity">
    /// The entity to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Removes multiple entities from the data set.
    /// </summary>
    /// <param name="entities">
    /// The entities to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task RemoveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the specified entity in the data set.
    /// </summary>
    /// <param name="entity">
    /// The entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
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
    Task UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value,
        CancellationToken cancellationToken);


    /// <summary>
    /// Updates multiple entities in the data set.
    /// </summary>
    /// <param name="entities">
    /// The entities to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
}