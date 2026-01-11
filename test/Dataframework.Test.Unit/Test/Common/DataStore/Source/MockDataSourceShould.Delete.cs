using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Source;

public partial class MockDataSourceShould
{
    [TestCase(false)]
    [TestCase(true)]
    public void Remove_One(bool doesExist)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(MockDataType1Collection);
        var entityToRemove = doesExist
            ? MockDataType1Collection[new Random().Next(MockDataType1Collection.Count)]
            : Create<MockDataType1>();
        MockDataType1? removalResult = null;

        // Act (define)
        var remove = () => MockDataSource.Remove<string, MockDataType1>(entityToRemove);
        var get = () => removalResult = MockDataSource.Get<string, MockDataType1>(entityToRemove.Id);

        // Assert
        remove.Should().NotThrow();
        get.Should().NotThrow();
        removalResult.Should().BeNull();
    }

    [Test]
    public void Remove_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? removalResult = null;

        // Act (define)
        var remove = () => MockDataSource.Remove<string, MockDataType1>((MockDataType1?)null!);
        var firstOrDefault = () => MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        remove.Should().NotThrow();
        firstOrDefault.Should().NotThrow();
        removalResult.Should().BeNull();
    }

    [TestCaseShift(3)]
    public void Remove_Many(bool isRemoveOne, bool isRemoveMany, bool isRemoveAll)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(MockDataType1Collection);
        var dataToRemove = TestArrangementHelper
            .GetRangeFrom(MockDataType1Collection, isRemoveOne, isRemoveMany, isRemoveAll).ToList();
        IEnumerable<MockDataType1>? removedResult = null;

        // Act (define)
        var remove = () => MockDataSource.Remove<string, MockDataType1>(dataToRemove);
        var where = () =>
            removedResult = MockDataSource.Where<string, MockDataType1>(data => dataToRemove.Contains(data));

        // Assert
        remove.Should().NotThrow();
        where.Should().NotThrow();
        removedResult.Should().BeEmpty();
    }

    [Test]
    public void Remove_Many_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? removedResults = null;

        // Act (define)
        var remove = () => MockDataSource.Remove<string, MockDataType1>((IEnumerable<MockDataType1>?)null!);
        var where = () => removedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        remove.Should().NotThrow();
        where.Should().NotThrow();
        removedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public void Remove_Many_When_Data_Not_Null(bool isOneNullRemove, bool isManyNullRemove)
    {
        // Arrange
        var nullDataToRemove = new List<MockDataType1?>(isOneNullRemove ? [null] : [null, null]);
        IEnumerable<MockDataType1>? removedResults = null;

        // Act (define)
        var remove = () => MockDataSource.Remove<string, MockDataType1>(nullDataToRemove!);
        var where = () => removedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        remove.Should().NotThrow();
        where.Should().NotThrow();
        removedResults.Should().BeEmpty();
    }
}