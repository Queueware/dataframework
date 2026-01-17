namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;

public class MockDataContextFactoryState
{
    public int CreateDataContextCallCount { get; set; }

    public void Reset() => CreateDataContextCallCount = 0;
}
