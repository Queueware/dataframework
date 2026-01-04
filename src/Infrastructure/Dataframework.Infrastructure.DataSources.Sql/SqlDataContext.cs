using Microsoft.EntityFrameworkCore;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Abstractions.DataSources.Sql;

namespace Queueware.Dataframework.Infrastructure.DataSources.Sql;

/// <summary>
/// Represents a SQL-backed implementation of <see cref="ISqlDataContext{TDbContext}"/>
/// that coordinates data set access and persistence using an underlying
/// Entity Framework Core <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TDbContext">
/// The concrete <see cref="DbContext"/> type used to interact with the
/// underlying SQL data store.
/// </typeparam>
/// <param name="dbContext">
/// The underlying Entity Framework Core <see cref="DbContext"/> instance
/// responsible for change tracking and persistence.
/// </param>
public class SqlDataContext<TDbContext>(TDbContext dbContext) : ISqlDataContext<TDbContext> where TDbContext : DbContext
{
    private bool _isDisposed;
    
    /// <summary>
    /// Releases all resources used by the data context.
    /// </summary>
    /// <remarks>
    /// Disposing the data context disposes the underlying
    /// <see cref="DbContext"/> instance and should be performed once the
    /// unit of work has completed.
    /// </remarks>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Retrieves a data set for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type managed by the returned data set.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IDataSet{TEntity}"/> that provides query and persistence
    /// operations for the specified entity type.
    /// </returns>
    public IDataSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return new SqlDataSet<TEntity>(dbContext.Set<TEntity>());
    }

    /// <summary>
    /// Persists all pending changes made within the data context to the
    /// underlying SQL data store.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to observe while waiting for the operation to complete.
    /// </param>
    /// <returns>
    /// The number of state entries written to the underlying data store.
    /// </returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private void Dispose(bool isDisposing)
    {
        if (!_isDisposed && isDisposing && dbContext != null!)
        {
            _isDisposed = true;
            dbContext.Dispose();
        }
    }
}