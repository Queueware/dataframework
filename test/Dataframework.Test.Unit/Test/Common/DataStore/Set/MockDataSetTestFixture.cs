using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Set;

public class MockDataSetTestFixture : CommonTestBase
{
    protected CancellationToken CancellationToken { get; set; }

    protected List<MockDataType1> DataSourceEntities { get; set; } = null!;

    protected MockDataSource MockDataSource { get; set; } = null!;

    protected MockDataSet<MockDataType1> MockDataSet { get; private set; } = null!;

    [SetUp]
    protected void SetUp()
    {
        CancellationToken = CancellationToken.None;
        DataSourceEntities = CreateMany<MockDataType1>().ToList();
        MockDataSource = new MockDataSource();
        MockDataSet = new MockDataSet<MockDataType1>(MockDataSource);
    }
}