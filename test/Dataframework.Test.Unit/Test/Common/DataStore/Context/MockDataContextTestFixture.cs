using NUnit.Framework;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Context;

public abstract class MockDataContextTestFixture : CommonTestBase
{
    protected CancellationToken CancellationToken;

    protected List<MockDataType1> MockDataType1Collection { get; set; } = null!;

    protected List<MockDataType2> MockDataType2Collection { get; set; } = null!;

    protected MockDataSource MockDataSource { get; set; }  = null!;

    protected MockDataContext MockDataContext { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        CancellationToken = CancellationToken.None;

        MockDataType1Collection = CreateMany<MockDataType1>().ToList();
        MockDataType2Collection = CreateMany<MockDataType2>().ToList();

        MockDataSource = new MockDataSource();
        MockDataSource.Add<string, MockDataType1>(MockDataType1Collection);
        MockDataSource.Add<string, MockDataType2>(MockDataType2Collection);

        MockDataContext = new MockDataContext(MockDataSource);
    }
}