using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Abstractions.Repositories;

/// <summary>
/// Defines a repository for querying and persisting entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TId">
/// The type of the entity identifier.
/// </typeparam>
/// <typeparam name="TEntity">
/// The entity type managed by the repository.
/// </typeparam>
public interface IRepository<in TId, TEntity> where TEntity : IId<TId>
{
    /// <summary>
    /// Returns the total number of entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// The total number of entities.
    /// </returns>
    Task<int> CountAsync(CancellationToken cancellationToken);

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
    /// Removes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">
    /// The entity to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// <c>true</c> if the entity was deleted; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Removes multiple entities from the repository.
    /// </summary>
    /// <param name="entities">
    /// The entities to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// <c>true</c> if the deletion occurred; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    
    /// <summary>
    /// Removes the entity of the specified identifier from the repository.
    /// </summary>
    /// <param name="id">
    /// The entity of the identifier to remove.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// <c>true</c> if the entity was deleted; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Removes multiple entities of the specified identifiers from the repository.
    /// </summary>
    /// <param name="ids">
    /// The entity identifiers.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// <c>true</c> if the deletion occurred; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken);

    /// <summary>
    /// Returns all entities that satisfy the specified predicate.
    /// </summary>
    /// <remarks>
    /// If no entities match the predicate, an empty collection is returned.
    /// The ordering of the returned entities is implementation-defined unless
    /// otherwise documented.
    /// </remarks>
    /// <param name="expression">
    /// A predicate used to filter the entities.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// A collection of entities that satisfy the specified predicate.
    /// </returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns the first entity that satisfies the specified predicate, or <c>null</c> if no such entity exists.
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
    /// Returns the entity with the specified identifier, or <c>null</c> if no such entity exists.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// The matching entity, or <c>null</c>.
    /// </returns>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    /// <summary>
    /// Returns the entities with the specified identifiers.
    /// </summary>
    /// <remarks>
    /// Implementations may return fewer entities than identifiers provided if some identifiers do not exist.
    /// The ordering of the returned entities is implementation-defined unless otherwise documented.
    /// </remarks>
    /// <param name="ids">
    /// The entity identifiers.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// A collection of entities matching the provided identifiers.
    /// </returns>
    Task<IEnumerable<TEntity>> GetByIdAsync(IEnumerable<TId> ids, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts the specified entity into the repository.
    /// </summary>
    /// <param name="entity">
    /// The entity to insert.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task<bool> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts multiple entities into the repository.
    /// </summary>
    /// <param name="entities">
    /// The entities to insert.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task<bool> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the specified entity in the repository.
    /// </summary>
    /// <param name="entity">
    /// The entity to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates multiple entities in the repository.
    /// </summary>
    /// <param name="entities">
    /// The entities to update.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    Task<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

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
    Task<bool> UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value, CancellationToken cancellationToken);
}
