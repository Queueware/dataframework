using System.Linq.Expressions;
using AutoFixture;
using FluentAssertions;
using LinqKit;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.DataSources;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Context;

[TestFixture]
public class MockDataContextShould : MockDataContextTestFixture
{
    [TestCaseShift(3, true)]
    public void Dispose(bool isDisposeOnce, bool isDisposeTwice, bool isDisposeMany)
    {
        // Arrange
        var expectedDisposeCallCount = isDisposeOnce ? 1 : isDisposeTwice ? 2 : isDisposeMany ? 3 : 0;

        // Act (define)
        var dispose = () => MockDataContext.Dispose();

        // Assert
        Enumerable.Range(0, expectedDisposeCallCount).ForEach(_ => dispose.Should().NotThrow());
        MockDataContext.State.DisposeCallCount.Should().Be(expectedDisposeCallCount);
    }

    [Test]
    public void Get_Set()
    {
        // Arrange
        IDataSet<MockDataType1>? result = null;

        // Act (define)
        var set = () => result = MockDataContext.Set<MockDataType1>();

        // Assert
        set.Should().NotThrow();
        result.Should().NotBeNull();
    }

    [Test]
    public async Task Get_Set_With_AddAsync_Of_One_Behavior()
    {
        // Arrange
        var dataToAdd = Create<MockDataType1>();
        MockDataType1? addedDataResult = null;

        // Act (define)
        var addAsync = async () =>
        {
            await MockDataContext.Set<MockDataType1>().AddAsync(dataToAdd, CancellationToken);
        };
        var get = () =>
        {
            addedDataResult = MockDataContext.MockDataSource.Get<string, MockDataType1>(dataToAdd.Id);
        };

        // Assert
        await addAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        addedDataResult.Should().Be(dataToAdd);
    }

    [Test]
    public async Task Get_Set_With_AddAsync_Of_Many_Behavior()
    {
        // Arrange
        var dataToAdd = CreateMany<MockDataType1>().ToList();
        IEnumerable<MockDataType1>? addedDataResult = null;

        // Act (define)
        var addAsync = async () =>
        {
            await MockDataContext.Set<MockDataType1>().AddAsync(dataToAdd, CancellationToken);
        };
        var getAddedData = () =>
        {
            addedDataResult = MockDataContext.MockDataSource
                .Where<string, MockDataType1>(data => dataToAdd.Contains(data));
        };

        // Assert
        await addAsync.Should().NotThrowAsync();
        getAddedData.Should().NotThrow();
        addedDataResult.Should().BeEquivalentTo(dataToAdd);
    }

    [Test]
    public void Get_Set_With_AsQueryableBehavior()
    {
        // Arrange
        var expectedDataResult = MockDataType1Collection;
        List<MockDataType1>? resultingDataResult = null;

        // Act (define)
        var asQueryable = () => resultingDataResult = MockDataContext.Set<MockDataType1>().AsQueryable().ToList();

        // Assert
        asQueryable.Should().NotThrow();
        resultingDataResult.Should().NotBeNull();
        resultingDataResult.Should().BeEquivalentTo(expectedDataResult);
        resultingDataResult.Should().NotBeSameAs(expectedDataResult);
        resultingDataResult
            .Zip(expectedDataResult, (expectedResult, result) => (expectedResult, result))
            .ForEach(pairedResults =>
            {
                pairedResults.result.Should().BeEquivalentTo(pairedResults.expectedResult)
                    .And
                    .NotBeSameAs(pairedResults.expectedResult);
            });
    }


    [TestCase(false, false)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    public async Task Get_Set_With_CountAsync_Behavior(bool hasSomeMatchedCount, bool hasAllMatchedCount)
    {
        // Arrange
        var name = Create<string>();
        var maxIndex = 0;
        if (hasSomeMatchedCount)
        {
            maxIndex = MockDataType1Collection.Count / 2;
        }
        else if (hasAllMatchedCount)
        {
            maxIndex = MockDataType1Collection.Count;
        }
        MockDataType1Collection[..maxIndex].ForEach(data =>
        {
            data.Name = name;
            MockDataSource.Update<string, MockDataType1>(data);
        });

        Expression<Func<MockDataType1, bool>> expression = data => data.Name == name;
        var expectedCountResult = MockDataType1Collection.Count(expression.Compile());
        int? countResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var countAsync = async () => countResult = await databaseSetResult!.CountAsync(expression, CancellationToken);

        // Assert
        set.Should().NotThrow();
        await countAsync.Should().NotThrowAsync();
        countResult.Should().Be(expectedCountResult);
    }

    [TestCaseShift(2)]
    public async Task Get_Set_With_FindAsync_Behavior(bool hasSomeMatchedCount, bool hasAllMatchedCount)
    {
        // Arrange
        var name = Create<string>();
        var maxIndex = 0;
        if (hasSomeMatchedCount)
        {
            maxIndex = MockDataType1Collection.Count / 2;
        }
        else if (hasAllMatchedCount)
        {
            maxIndex = MockDataType1Collection.Count;
        }
        MockDataType1Collection[..maxIndex].ForEach(data =>
        {
            data.Name = name;
            MockDataSource.Update<string, MockDataType1>(data);
        });

        Expression<Func<MockDataType1, bool>> expression = data => data.Name == name;
        var expectedResult = MockDataType1Collection.Where(expression.Compile());
        IEnumerable<MockDataType1>? findResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var findAsync = async () => findResult = await databaseSetResult!.FindAsync(expression, CancellationToken);

        // Assert
        set.Should().NotThrow();
        await findAsync.Should().NotThrowAsync();
        findResult.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task Get_Set_With_FirstOrDefaultAsync_Behavior(bool hasMatch)
    {
        // Arrange
        var name = Create<string>();
        if (hasMatch)
        {
            var data = MockDataType1Collection[new Random().Next(0, MockDataType1Collection.Count)];
            data.Name = name;
            MockDataSource.Update<string, MockDataType1>(data);
        }

        Expression<Func<MockDataType1, bool>> expression = data => data.Name == name;
        var expectedResult = MockDataType1Collection.FirstOrDefault(expression.Compile());
        MockDataType1? firstResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var firstOrDefaultAsync = async () => firstResult = await databaseSetResult!.FirstOrDefaultAsync(expression, CancellationToken);

        // Assert
        set.Should().NotThrow();
        await firstOrDefaultAsync.Should().NotThrowAsync();
        firstResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task Get_Set_With_RemoveAsync_Of_One_Behavior()
    {
        // Arrange
        var dataToRemove = MockDataType1Collection[new Random().Next(0, MockDataType1Collection.Count)];
        MockDataType1? removedResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var removeAsync = async () => await databaseSetResult!.RemoveAsync(dataToRemove, CancellationToken);
        var get = () =>
        {
            removedResult = MockDataContext.MockDataSource
                .FirstOrDefault<string, MockDataType1>(data => data.Id == dataToRemove.Id);
        };

        // Assert
        set.Should().NotThrow();
        await removeAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        removedResult.Should().BeNull();
    }

    [Test]
    public async Task Get_Set_With_RemoveAsync_Of_Many_Behavior()
    {
        // Arrange
        var random = new Random();
        List<MockDataType1> dataToRemove =
        [
            MockDataType1Collection[random.Next(0, MockDataType1Collection.Count / 2)],
            MockDataType1Collection[random.Next(MockDataType1Collection.Count / 2, MockDataType1Collection.Count)]
        ];
        IEnumerable<MockDataType1>? removedResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var removeAsync = async () => await databaseSetResult!.RemoveAsync(dataToRemove, CancellationToken);
        var get = () =>
        {
            removedResult = MockDataContext.MockDataSource
                .Where<string, MockDataType1>(data => dataToRemove.Contains(data));
        };

        // Assert
        set.Should().NotThrow();
        await removeAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        removedResult.Should().BeEmpty();
    }

    [Test]
    public async Task Get_Set_With_UpdateAsync_Of_One_Behavior()
    {
        // Arrange
        var dataToUpdate = MockDataType1Collection[new Random().Next(0, MockDataType1Collection.Count)];
        dataToUpdate.Name = Create<string>();
        MockDataType1? updatedResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var updateAsync = async () => await databaseSetResult!.UpdateAsync(dataToUpdate, CancellationToken);
        var get = () =>
        {
            updatedResult = MockDataContext.MockDataSource
                .FirstOrDefault<string, MockDataType1>(data => data.Equals(dataToUpdate));
        };

        // Assert
        set.Should().NotThrow();
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        updatedResult.Should().BeEquivalentTo(dataToUpdate);
    }

    [Test]
    public async Task Get_Set_With_UpdateAsync_Of_Many_Behavior()
    {
        // Arrange
        var random = new Random();
        List<MockDataType1> dataToUpdate =
        [
            MockDataType1Collection[random.Next(0, MockDataType1Collection.Count / 2)],
            MockDataType1Collection[random.Next(MockDataType1Collection.Count / 2, MockDataType1Collection.Count)]
        ];
        dataToUpdate.ForEach(data => data.Name = Create<string>());
        IEnumerable<MockDataType1>? updatedResult = null;
        IDataSet<MockDataType1>? databaseSetResult = null;

        // Act (define)
        var set = () => databaseSetResult = MockDataContext.Set<MockDataType1>();
        var updateAsync = async () => await databaseSetResult!.UpdateAsync(dataToUpdate, CancellationToken);
        var get = () =>
        {
            updatedResult = MockDataContext.MockDataSource
                .Where<string, MockDataType1>(data => dataToUpdate.Contains(data));
        };

        // Assert
        set.Should().NotThrow();
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        updatedResult.Should().BeEquivalentTo(dataToUpdate);
    }
}