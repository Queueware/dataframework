using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;

public class MockDataContextFactory(MockDataContext mockDataContext) : IDataContextFactory<MockDataContext>
{
    public readonly MockDataContextFactoryState State = new();

    public MockDataContext MockDataContext { get; } = mockDataContext;

    public MockDataContext CreateDataContext()
    {
        State.CreateDataContextCallCount++;
        return MockDataContext;
    }
}
