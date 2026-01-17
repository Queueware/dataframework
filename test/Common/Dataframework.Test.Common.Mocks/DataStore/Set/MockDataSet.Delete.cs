namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;

public partial class MockDataSet<TEntity> where TEntity : class
{
    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Remove), IsGenericSingleParameterizedSignature)
            .Invoke(MockDataSource, [entity]));
    }

    public Task RemoveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Remove),
                IsGenericEnumerableParameterizedSignature)
            .Invoke(MockDataSource, [entities]));
    }
}