using AutoFixture;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Source;

public abstract class MockDataSourceTestFixture : CommonTestBase
{
    protected List<MockDataType1> MockDataType1Collection { get; set; } = null!;

    protected List<MockDataType2> MockDataType2Collection { get; set; } = null!;

    protected MockDataSource MockDataSource { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        MockDataType1Collection = CreateMany<MockDataType1>().ToList();
        MockDataType2Collection = CreateMany<MockDataType2>().ToList();

        MockDataSource = new MockDataSource();
    }
}