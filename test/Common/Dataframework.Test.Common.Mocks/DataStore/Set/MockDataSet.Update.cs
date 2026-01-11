using System.Linq.Expressions;
using System.Reflection;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;

public partial class MockDataSet<TEntity> where TEntity : class
{
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var boundUpdateAsync = _mockDataSourceBinder.CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Update),
            IsGenericSingleParameterizedSignature);
        return Task.FromResult(boundUpdateAsync.Invoke(MockDataSource, [entity]));
    }

    public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Update), SignatureSelector)
            .Invoke(MockDataSource, [entities]));

        bool SignatureSelector(ParameterInfo[] parameterInfo)
        {
            return IsGenericEnumerableParameterizedSignature(parameterInfo) && parameterInfo.Length == 1;
        }
    }

    public Task UpdateAsync<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value, CancellationToken cancellationToken)
    {
        var updateByFieldMethodInfo = _mockDataSourceBinder
            .CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Update), IsGenericFieldValueSignature,
                typeof(TField));

        return Task.FromResult(updateByFieldMethodInfo.Invoke(MockDataSource,
            [MockDataSourceBinder.GetEntityIdValue(entity), field, value]));
    }
}