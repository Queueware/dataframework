using Microsoft.EntityFrameworkCore;
using Queueware.Dataframework.Abstractions.DataSources.Sql;

namespace Queueware.Dataframework.Infrastructure.DataSources.Sql;

/// <summary>
/// Represents a factory responsible for creating SQL-backed data contexts
/// implemented using Entity Framework Core <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TDbContext">
/// The concrete <see cref="DbContext"/> type created by the underlying
/// <see cref="IDbContextFactory{TContext}"/> and used by the produced
/// <see cref="ISqlDataContext{TDbContext}"/> instances.
/// </typeparam>
/// <param name="dbContextFactory">
/// The Entity Framework Core <see cref="IDbContextFactory{TContext}"/> used to create
/// <typeparamref name="TDbContext"/> instances.
/// </param>
public class SqlDataContextFactory<TDbContext>(IDbContextFactory<TDbContext> dbContextFactory) : ISqlDataContextFactory<TDbContext>
    where TDbContext : DbContext 
{
    /// <summary>
    /// Creates a new SQL-backed data context instance.
    /// </summary>
    /// <remarks>
    /// Each invocation should return a new, independent data context suitable for a
    /// single unit of work. The caller is responsible for disposing the returned
    /// <see cref="ISqlDataContext{TDbContext}"/> when finished.
    /// </remarks>
    /// <returns>
    /// A newly created <see cref="ISqlDataContext{TDbContext}"/>.
    /// </returns>
    public ISqlDataContext<TDbContext> CreateDataContext()
    {
        return new SqlDataContext<TDbContext>(dbContextFactory.CreateDbContext());
    }
}