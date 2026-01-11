using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Infrastructure.Repositories;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Infrastructure.Infrastructure.Repositories;

public abstract class RepositoryTestFixture : CommonTestBase
{
    protected CancellationToken CancellationToken { get; private set; }

    protected List<MockDataType1> RepositoryEntities { get; private set; } = null!;

    protected MockDataSource MockDataSource { get; private set; } = null!;

    protected MockDataContext MockDataContext { get; private set; } = null!;

    protected MockDataContextFactory MockDataContextFactory { get; private set; } = null!;

    protected Repository<string, MockDataType1, IDataContext> Repository { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        CancellationToken = CancellationToken.None;
        RepositoryEntities = CreateMany<MockDataType1>().ToList();

        MockDataSource = new MockDataSource();
        MockDataSource.Add<string, MockDataType1>(RepositoryEntities);
        MockDataContext = new MockDataContext(MockDataSource) { CancellationToken = CancellationToken };
        MockDataContextFactory = new MockDataContextFactory(MockDataContext);

        Repository = new Repository<string, MockDataType1, IDataContext>(MockDataContextFactory);
    }

    protected void VerifyDataContextCreationAndDisposalCalls()
    {
        MockDataContextFactory.State.CreateDataContextCallCount.Should().Be(1);
        MockDataContext.State.DisposeCallCount.Should().Be(1);
    }
}