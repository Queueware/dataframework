namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;

public class MockDataContextState
{
    public int DisposeCallCount { get; set; }

    public int SaveChangesAsyncCallCount { get; set; }

    public int SaveChangesAsyncReturnValue { get; set; }

    public void Reset()
    {
        DisposeCallCount = 0;
        SaveChangesAsyncCallCount = 0;
        SaveChangesAsyncReturnValue = 0;
    }
}
