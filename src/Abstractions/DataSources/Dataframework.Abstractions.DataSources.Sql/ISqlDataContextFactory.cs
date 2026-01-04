using Microsoft.EntityFrameworkCore;

namespace Queueware.Dataframework.Abstractions.DataSources.Sql;

/// <summary>
/// Defines a factory responsible for creating SQL-backed data contexts
/// implemented using Entity Framework Core <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TDbContext">
/// The concrete <see cref="DbContext"/> type used by the created
/// <see cref="ISqlDataContext{TDbContext}"/> instances.
/// </typeparam>
public interface ISqlDataContextFactory<TDbContext> : IDataContextFactory<ISqlDataContext<TDbContext>> where TDbContext : DbContext;
