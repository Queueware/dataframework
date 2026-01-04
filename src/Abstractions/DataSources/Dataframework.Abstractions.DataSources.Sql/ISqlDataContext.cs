using Microsoft.EntityFrameworkCore;

namespace Queueware.Dataframework.Abstractions.DataSources.Sql;

/// <summary>
/// Represents a SQL-backed <see cref="IDataContext"/> that is implemented
/// using an underlying Entity Framework Core <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TDbContext">
/// The concrete <see cref="DbContext"/> type used to interact with the
/// underlying SQL data store.
/// </typeparam>
public interface ISqlDataContext<TDbContext> : IDataContext where TDbContext : DbContext;