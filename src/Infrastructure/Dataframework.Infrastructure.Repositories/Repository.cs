using System.Linq.Expressions;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Abstractions.Primitives;
using Queueware.Dataframework.Abstractions.Repositories;

namespace Queueware.Dataframework.Infrastructure.Repositories;

/// <inheritdoc />
public partial class Repository<TId, TEntity, TDataContext>(IDataContextFactory<TDataContext> dataContextFactory)
    : IRepository<TId, TEntity> where TEntity : class, IId<TId> where TDataContext : class, IDataContext
{
    private IDataContext _dataContext() => dataContextFactory.CreateDataContext();
}