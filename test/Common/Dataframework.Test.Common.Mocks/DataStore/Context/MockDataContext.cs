using System.Diagnostics.CodeAnalysis;
using LinqKit;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;

public class MockDataContext : IDataContext
{
    private readonly IDictionary<Type, object> _databaseSetStorage;

    public readonly MockDataContextState State = new();

    public CancellationToken CancellationToken { get; set; }

    public required MockDataSource MockDataSource;

    [SetsRequiredMembers]
    public MockDataContext (MockDataSource mockDataSource)
    {
        MockDataSource = mockDataSource;
        _databaseSetStorage = new Dictionary<Type, object>();
        BuildDataSetStorage();
    }

    public void Dispose()
    {
        State.DisposeCallCount++;
        GC.SuppressFinalize(this);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        State.SaveChangesAsyncCallCount++;
        return Task.FromResult(State.SaveChangesAsyncReturnValue);
    }

    public IDataSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return (IDataSet<TEntity>)_databaseSetStorage[typeof(TEntity)];
    }

    private void BuildDataSetStorage()
    {
        var mockDataSetGenericType = typeof(MockDataSet<>);
        MockDataSource.StoredTypes.ForEach(storedType =>
        {
            var boundType = mockDataSetGenericType.MakeGenericType(storedType);
            var constructor = boundType.GetConstructor([typeof(MockDataSource)]);
            if (constructor == null)
            {
                const string MissingConstructor =
                    $"No constructor found for {nameof(boundType)} with single {nameof(MockDataSource)} parameter";
                throw new MissingMethodException(MissingConstructor);
            }

            _databaseSetStorage[storedType] = constructor.Invoke([MockDataSource]);
        });
    }
}
