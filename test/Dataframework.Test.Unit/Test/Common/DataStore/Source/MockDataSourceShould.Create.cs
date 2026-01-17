using FluentAssertions;
using LinqKit;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Source;

public partial class MockDataSourceShould
{
    [TestCaseShift(3)]
    public void Add_One(bool isAddOne, bool isAddMany, bool isAddAll)
    {
        // Arrange
        var dataToAdd = TestArrangementHelper.GetRangeFrom(MockDataType1Collection, isAddOne, isAddMany, isAddAll);
        IEnumerable<MockDataType1>? addedResult = null;

        // Act (define)
        var add = () => dataToAdd.ForEach(data => MockDataSource.Add<string, MockDataType1>(data));
        var where = () => addedResult = MockDataSource.Where<string, MockDataType1>(data => dataToAdd.Contains(data));

        // Assert
        add.Should().NotThrow();
        where.Should().NotThrow();
        addedResult.Should().BeEquivalentTo(dataToAdd);
    }

    [Test]
    public void Add_One_As_Copy()
    {
        // Arrange
        var dataToAdd = MockDataType1Collection.First();
        MockDataType1? dataAddedResult = null;

        // Act (define)
        var add = () => MockDataSource.Add<string, MockDataType1>(dataToAdd);
        var firstOrDefault = () =>
            dataAddedResult = MockDataSource.FirstOrDefault<string, MockDataType1>(entity => entity.Id == dataToAdd.Id);

        // Assert
        add.Should().NotThrow();
        firstOrDefault.Should().NotThrow();
        dataAddedResult.Should().NotBeSameAs(dataToAdd);
        dataAddedResult.Should().BeEquivalentTo(dataToAdd);
    }

    [Test]
    public void Add_One_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var add = () => MockDataSource.Add<string, MockDataType1>((MockDataType1?)null!);
        var where = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        add.Should().NotThrow();
        where.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }

    [TestCaseShift(3)]
    public void Add_Many(bool isAddOne, bool isAddMany, bool isAddAll)
    {
        // Arrange
        var dataToAdd = TestArrangementHelper.GetRangeFrom(MockDataType1Collection, isAddOne, isAddMany, isAddAll);
        IEnumerable<MockDataType1>? addedResult = null;

        // Act (define)
        var add = () => MockDataSource.Add<string, MockDataType1>(dataToAdd);
        var where = () => addedResult = MockDataSource.Where<string, MockDataType1>(data => dataToAdd.Contains(data));

        // Assert
        add.Should().NotThrow();
        where.Should().NotThrow();
        addedResult.Should().BeEquivalentTo(dataToAdd);
    }

    [Test]
    public void Add_Many_As_Copy()
    {
        // Arrange
        List<MockDataType1>? dataAddedResult = null;

        // Act (define)
        var add = () => MockDataSource.Add<string, MockDataType1>(MockDataType1Collection);
        var where = () =>
        {
            return dataAddedResult = MockDataSource
                .Where<string, MockDataType1>(entity => MockDataType1Collection.Contains(entity))
                .OrderBy(entity => entity.Id)
                .ToList();
        };

        // Assert
        add.Should().NotThrow();
        where.Should().NotThrow();
        MockDataType1Collection = MockDataType1Collection.OrderBy(entity => entity.Id).ToList();
        dataAddedResult.Should().NotBeNull();
        dataAddedResult = dataAddedResult.OrderBy(entity => entity.Id).ToList();
        dataAddedResult.Should().NotBeSameAs(MockDataType1Collection);
        dataAddedResult.Should().BeEquivalentTo(MockDataType1Collection);
    }

    [Test]
    public void Add_Many_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var add = () => MockDataSource.Add<string, MockDataType1>((IEnumerable<MockDataType1>?)null!);
        var where = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        add.Should().NotThrow();
        where.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public void Add_Many_When_Data_Not_Null(bool isOneNullAdd, bool isManyNullAdd)
    {
        // Arrange
        var nullDataToAdd = new List<MockDataType1?>(isOneNullAdd ? [null] : [null, null]);
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var add = () => MockDataSource.Add<string, MockDataType1>(nullDataToAdd!);
        var where = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        add.Should().NotThrow();
        where.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }
}