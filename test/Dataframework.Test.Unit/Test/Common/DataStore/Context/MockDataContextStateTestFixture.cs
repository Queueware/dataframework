using NUnit.Framework;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Context;

public abstract class MockDataContextStateTestFixture : CommonTestBase
{
    protected MockDataContextState MockDataContextState { get; set; } = null!;

    [SetUp]
    public void SetUp()
    {
        MockDataContextState = new MockDataContextState();
    }
}
