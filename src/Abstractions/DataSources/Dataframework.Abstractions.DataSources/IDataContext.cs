namespace Queueware.Dataframework.Abstractions.DataSources;

/// <summary>
/// Represents a unit-of-work–style data context responsible for
/// managing entity sets and coordinating persistence operations.
/// </summary>
public interface IDataContext : IDisposable
{
    /// <summary>
    /// Retrieves a data set for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type managed by the returned data set.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDataSet{TEntity}"/> that provides access to
    /// querying and persistence operations for the entity type.
    /// </returns>
    IDataSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Persists all pending changes made within the context to the
    /// underlying data store.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// The number of state entries written to the underlying data store.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
