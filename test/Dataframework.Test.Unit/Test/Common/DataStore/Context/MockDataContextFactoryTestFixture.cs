using NUnit.Framework;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Context;

public abstract class MockDataContextFactoryTestFixture : CommonTestBase
{
    protected MockDataSource MockDataSource { get; set; } = null!;

    protected MockDataContext MockDataContext { get; set; } = null!;

    protected MockDataContextFactory MockDataContextFactory { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        MockDataSource = new MockDataSource();
        MockDataContext = new MockDataContext(MockDataSource);
        MockDataContextFactory = new MockDataContextFactory(MockDataContext);
    }
}