using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Context;

[TestFixture]
public class MockDataContextStateShould : MockDataContextStateTestFixture
{
    [Test]
    public void Get_DisposeCallCount()
    {
        // Arrange
        const int ExpectedResult = 0;
        int? result = null;

        // Act (define)
        var get = () => result = MockDataContextState.DisposeCallCount;

        // Assert
        get.Should().NotThrow();
        result.Should().Be(ExpectedResult);
    }

    [TestCaseRange(-1, 2)]
    public void Set_DisposeCallCount(int expectedResult)
    {
        // Arrange
        int? result = null;

        // Act (define)
        var set = () => MockDataContextState.DisposeCallCount = expectedResult;
        var get = () => result = MockDataContextState.DisposeCallCount;

        // Assert
        set.Should().NotThrow();
        get.Should().NotThrow();
        result.Should().Be(expectedResult);
    }

    [Test]
    public void Get_SaveChangesAsyncCallCount()
    {
        // Arrange
        const int ExpectedResult = 0;
        int? result = null;

        // Act (define)
        var get = () => result = MockDataContextState.SaveChangesAsyncCallCount;

        // Assert
        get.Should().NotThrow();
        result.Should().Be(ExpectedResult);
    }

    [TestCaseRange(-1, 2)]
    public void Set_SaveChangesAsyncCallCount(int expectedResult)
    {
        // Arrange
        int? result = null;

        // Act (define)
        var set = () => MockDataContextState.SaveChangesAsyncCallCount = expectedResult;
        var get = () => result = MockDataContextState.SaveChangesAsyncCallCount;

        // Assert
        set.Should().NotThrow();
        get.Should().NotThrow();
        result.Should().Be(expectedResult);
    }

    [Test]
    public void Get_SaveChangesAsyncReturnValue()
    {
        // Arrange
        const int ExpectedResult = 0;
        int? result = null;

        // Act (define)
        var get = () => result = MockDataContextState.SaveChangesAsyncReturnValue;

        // Assert
        get.Should().NotThrow();
        result.Should().Be(ExpectedResult);
    }

    [Test]
    public void Set_SaveChangesAsyncReturnValue([Range(-1, 2)] int expectedResult)
    {
        // Arrange
        int? result = null;

        // Act (define)
        var set = () => MockDataContextState.SaveChangesAsyncReturnValue = expectedResult;
        var get = () => result = MockDataContextState.SaveChangesAsyncReturnValue;

        // Assert
        set.Should().NotThrow();
        get.Should().NotThrow();
        result.Should().Be(expectedResult);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Reset(bool isResetFromDefault)
    {
        // Arrange
        const int ExpectedDisposeCallCount = 0;
        const int ExpectedSaveChangesAsyncCallCount = 0;
        const int ExpectedSaveChangesAsyncReturnValue = 0;

        // Act (define)
        var setAll = () =>
        {
            if (!isResetFromDefault)
            {
                MockDataContextState.DisposeCallCount++;
                MockDataContextState.SaveChangesAsyncCallCount++;
                MockDataContextState.SaveChangesAsyncReturnValue++;
            }
        };
        var reset = () => MockDataContextState.Reset();

        // Assert
        setAll.Should().NotThrow();
        reset.Should().NotThrow();
        MockDataContextState.DisposeCallCount.Should().Be(ExpectedDisposeCallCount);
        MockDataContextState.SaveChangesAsyncCallCount.Should().Be(ExpectedSaveChangesAsyncCallCount);
        MockDataContextState.SaveChangesAsyncReturnValue.Should().Be(ExpectedSaveChangesAsyncReturnValue);
    }
}