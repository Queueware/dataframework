using System.Diagnostics.CodeAnalysis;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;

[method: SetsRequiredMembers]
public partial class MockDataSet<TEntity>(MockDataSource mockDataSource) : IDataSet<TEntity> where TEntity : class
{
    private readonly MockDataSourceBinder _mockDataSourceBinder = new(mockDataSource);

    public required MockDataSource MockDataSource { get; set; } = mockDataSource;
}