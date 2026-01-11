namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;

public partial class MockDataSet<TEntity> where TEntity : class
{
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Add), IsGenericSingleParameterizedSignature)
            .Invoke(MockDataSource, [entity]));
    }

    public Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Add), IsGenericEnumerableParameterizedSignature)
            .Invoke(MockDataSource, [entities]));
    }
}