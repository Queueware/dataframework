using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.DataSources;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Context;

[TestFixture]
public class MockDataContextFactoryShould : MockDataContextFactoryTestFixture
{
    [Test]
    public void CreateDataContext()
    {
        // Arrange
        IDataContext? result = null;

        // Act (define)
        var createDataContext = () => result = MockDataContextFactory.CreateDataContext();

        // Assert
        createDataContext.Should().NotThrow();
        result.Should().NotBeNull();
    }

    [Test]
    public void CreateDataContext_With_Same_Instance()
    {
        // Arrange
        IDataContext? firstResult = null;
        IDataContext? secondResult = null;

        // Act (define)
        var createFirstDataContext = () => firstResult = MockDataContextFactory.CreateDataContext();
        var createSecondDataContext = () => secondResult = MockDataContextFactory.CreateDataContext();

        // Assert
        createFirstDataContext.Should().NotThrow();
        firstResult.Should().NotBeNull();

        createSecondDataContext.Should().NotThrow();
        secondResult.Should().NotBeNull();

        firstResult.Should().Be(secondResult);
    }
}