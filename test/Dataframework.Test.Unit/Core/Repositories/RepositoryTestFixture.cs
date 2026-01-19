using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Context;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Repositories;

public abstract class RepositoryTestFixture : CommonTestBase
{
    protected CancellationToken CancellationToken { get; private set; }

    protected List<MockDataType1> RepositoryEntities { get; private set; } = null!;

    protected MockDataSource MockDataSource { get; private set; } = null!;

    protected MockDataContext MockDataContext { get; private set; } = null!;

    protected MockDataContextFactory MockDataContextFactory { get; private set; } = null!;

    protected Dataframework.Core.Repositories.Repository<string, MockDataType1, IDataContext> Repository { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        CancellationToken = CancellationToken.None;
        RepositoryEntities = CreateMany<MockDataType1>().ToList();

        MockDataSource = new MockDataSource();
        MockDataSource.Add<string, MockDataType1>(RepositoryEntities);
        MockDataContext = new MockDataContext(MockDataSource) { CancellationToken = CancellationToken };
        MockDataContextFactory = new MockDataContextFactory(MockDataContext);

        Repository = new Dataframework.Core.Repositories.Repository<string, MockDataType1, IDataContext>(MockDataContextFactory);
    }

    protected void VerifyDataContextCreationAndDisposalCalls(int callCount)
    {
        MockDataContextFactory.State.CreateDataContextCallCount.Should().Be(callCount);
        MockDataContext.State.DisposeCallCount.Should().Be(callCount);
    }
}