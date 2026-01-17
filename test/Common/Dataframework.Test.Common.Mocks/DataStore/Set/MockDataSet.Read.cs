using System.Linq.Expressions;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;

public partial class MockDataSet<TEntity> where TEntity : class
{
    public IQueryable<TEntity> AsQueryable()
    {
        return (_mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Get), IsVoidParameterizedSignature)
            .Invoke(MockDataSource, []) as IEnumerable<TEntity> ?? []).AsQueryable();
    }
    
    public Task<int> CountAsync(CancellationToken cancellationToken) => CountAsync(_ => true, cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        var boundWhereResult = _mockDataSourceBinder.CreateMockDataSourceWhereBinding<TEntity>()
            .Invoke(MockDataSource, [expression.Compile()]);
        return Task.FromResult((boundWhereResult as IEnumerable<TEntity> ?? []).Count());
    }

    public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        var boundWhereResult = _mockDataSourceBinder.CreateMockDataSourceWhereBinding<TEntity>()
            .Invoke(MockDataSource, [expression.Compile()]);
        return Task.FromResult(boundWhereResult as IEnumerable<TEntity> ?? []);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken)
    {
        var boundFirstOrDefaultAsync =
            _mockDataSourceBinder.CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.FirstOrDefault));
        return Task.FromResult((TEntity?)boundFirstOrDefaultAsync.Invoke(MockDataSource, [expression.Compile()]));
    }
}