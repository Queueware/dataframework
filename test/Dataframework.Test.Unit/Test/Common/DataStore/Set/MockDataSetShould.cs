using System.Linq.Expressions;
using FluentAssertions;
using Force.DeepCloner;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Test.Common.DataStore.Set;

[TestFixture]
public class MockDataSetShould : MockDataSetTestFixture
{
    [TestCaseShift(3)]
    public void Return_AsQueryable(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var expectedResult = TestArrangementHelper.GetRangeFrom(DataSourceEntities, isOne, isMany, isAll).ToList();
        MockDataSource.Add<string, MockDataType1>(expectedResult);
        IQueryable<MockDataType1>? result = null;

        // Act (define)
        var asQueryable = () => result = MockDataSet.AsQueryable();

        // Assert
        asQueryable.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task AddAsync_Of_One(bool isNull)
    {
        // Arrange
        var entityToAdd = !isNull ? DataSourceEntities.First() : null;
        MockDataType1? entityAdded = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync(entityToAdd!, CancellationToken);
        var getAddedEntity = () =>
        {
            entityAdded = MockDataSource.FirstOrDefault<string, MockDataType1>(entity => entity.Id == entityToAdd?.Id);
        };

        // Assert
        await addAsync.Should().NotThrowAsync();
        getAddedEntity.Should().NotThrow();
        var entityAddedShould = entityAdded.Should();
        if (!isNull)
        {
            entityAddedShould.NotBeSameAs(entityToAdd);
            entityAddedShould.BeEquivalentTo(entityToAdd);
        }
        else
        {
            entityAddedShould.BeNull();
        }
    }

    [Test]
    public async Task AddAsync_Of_One_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync((MockDataType1?)null!, CancellationToken);
        var getNullEntities = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await addAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }

    [TestCaseRange(0, 2)]
    public async Task AddAsync_Of_Many(int addCount)
    {
        // Arrange
        var entitiesToAdd = DataSourceEntities[..addCount];
        IEnumerable<MockDataType1>? entitiesAdded = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync(entitiesToAdd, CancellationToken);
        var getAddedEntities = () =>
        {
            entitiesAdded = MockDataSource.Where<string, MockDataType1>(entity => entitiesToAdd.Contains(entity));
        };

        // Assert
        await addAsync.Should().NotThrowAsync();
        getAddedEntities.Should().NotThrow();
        entitiesAdded.Should().BeEquivalentTo(entitiesToAdd);
    }

    [Test]
    public async Task AddAsync_Of_Many_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync((IEnumerable<MockDataType1>?)null!, CancellationToken);
        var getNullEntities = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await addAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public async Task AddAsync_Of_Many_When_Data_Not_Null(bool isOneNullAdd, bool isManyNullAdd)
    {
        // Arrange
        var nullDataToAdd = new List<MockDataType1?>(isOneNullAdd ? [null] : [null, null]);
        IEnumerable<MockDataType1>? addedResults = null;

        // Act (define)
        var addAsync = async () => await MockDataSet.AddAsync(nullDataToAdd!, CancellationToken);
        var getNullEntities = () => addedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await addAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        addedResults.Should().BeEmpty();
    }
    
    [TestCaseRange(0, 2)]
    public async Task CountAsync(int expectedResult)
    {
        // Arrange
        DataSourceEntities = DataSourceEntities[..expectedResult];
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        
        int? result = null;

        // Act (define)
        var countAsync = async () => result = await MockDataSet.CountAsync(CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
    }

    [TestCaseRange(0, 2)]
    public async Task CountAsync_By_Expression(int expectedResult)
    {
        // Arrange
        var name = Create<string>();
        DataSourceEntities[..expectedResult].ForEach(entity => entity.Name = name);
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        int? result = null;

        // Act (define)
        var countAsync = async () => result = await MockDataSet.CountAsync(expression, CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
    }

    [TestCaseRange(0, 2)]
    public async Task FindAsync(int matchCount)
    {
        // Arrange
        var name = Create<string>();
        var entitiesToMatch = DataSourceEntities[..matchCount].OrderBy(entity => entity.Id).ToList();
        entitiesToMatch.ForEach(entity => entity.Name = name);
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        IEnumerable<MockDataType1>? result = null;

        // Act (define)
        var findAsync = async () =>
        {
            result = (await MockDataSet.FindAsync(expression, CancellationToken)).OrderBy(entity => entity.Id);
        };

        // Assert
        await findAsync.Should().NotThrowAsync();
        result.Should().BeEquivalentTo(entitiesToMatch);
    }

    [TestCaseRange(0, 2)]
    public async Task FirstOrDefaultAsync(int matchCount)
    {
        // Arrange
        var name = Create<string>();
        var entitiesToMatch = DataSourceEntities[..matchCount];
        entitiesToMatch.ForEach(entity => entity.Name = name);
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);

        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        var expectedResult = entitiesToMatch.FirstOrDefault(expression.Compile());
        MockDataType1? result = null;

        // Act (define)
        var firstOrDefaultAsync = async () =>
        {
            return result = await MockDataSet.FirstOrDefaultAsync(expression, CancellationToken);
        };

        // Assert
        await firstOrDefaultAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task RemoveAsync_Of_One(bool doesExist)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entityToRemove = doesExist
            ? DataSourceEntities[new Random().Next(DataSourceEntities.Count)]
            : Create<MockDataType1>();
        MockDataType1? removalResult = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync(entityToRemove, CancellationToken);
        var get = () => removalResult = MockDataSource.Get<string, MockDataType1>(entityToRemove.Id);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        removalResult.Should().BeNull();
    }

    [Test]
    public async Task RemoveAsync_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? removalResult = null;

        // Act (define)
        var removeAsync = () => MockDataSet.RemoveAsync((MockDataType1?)null!, CancellationToken);
        var firstOrDefault = () => MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        firstOrDefault.Should().NotThrow();
        removalResult.Should().BeNull();
    }

    [TestCaseShift(3)]
    public async Task RemoveAsync_Of_Many(bool isRemoveOne, bool isRemoveMany, bool isRemoveAll)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var dataToRemove = TestArrangementHelper.GetRangeFrom(DataSourceEntities, isRemoveOne, isRemoveMany, isRemoveAll).ToList();
        IEnumerable<MockDataType1>? removedResult = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync(dataToRemove, CancellationToken);
        var where = () => removedResult = MockDataSource.Where<string, MockDataType1>(data => dataToRemove.Contains(data));

        // Assert
        await removeAsync.Should().NotThrowAsync();
        where.Should().NotThrow();
        removedResult.Should().BeEmpty();
    }

    [Test]
    public async Task RemoveAsync_Of_Many_When_Not_Null()
    {
        // Arrange
        IEnumerable<MockDataType1>? removedResults = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync((IEnumerable<MockDataType1>?)null!, CancellationToken);
        var where = () => removedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        where.Should().NotThrow();
        removedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public async Task RemoveAsync_Of_Many_When_Data_Not_Null(bool isOneNullRemove, bool isManyNullRemove)
    {
        // Arrange
        var nullDataToRemove = new List<MockDataType1?>(isOneNullRemove ? [null] : [null, null]);
        IEnumerable<MockDataType1>? removedResults = null;

        // Act (define)
        var removeAsync = async () => await MockDataSet.RemoveAsync(nullDataToRemove!, CancellationToken);
        var where = () => removedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await removeAsync.Should().NotThrowAsync();
        where.Should().NotThrow();
        removedResults.Should().BeEmpty();
    }


    [TestCase(false, false)]
    [TestCase(true, false)]
    [TestCase(true, true)]
    public async Task UpdateAsync_Of_One(bool doesExist, bool isUpdate)
    {
        // Arrange
        var dataSourceEntities = DataSourceEntities;
        var entityToUpdate = DataSourceEntities.First();
        if (!doesExist)
        {
            dataSourceEntities = dataSourceEntities[1..];
        }
        MockDataSource.Add<string, MockDataType1>(dataSourceEntities);

        if (isUpdate)
        {
            entityToUpdate.Name = Create<string>();
        }

        var expectedUpdatedResult = doesExist ? entityToUpdate : null;
        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entityToUpdate, CancellationToken);
        var getUpdatedEntity = () => updatedResult = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getUpdatedEntity.Should().NotThrow();
        updatedResult.Should().BeEquivalentTo(expectedUpdatedResult);
    }

    [Test]
    public async Task UpdateAsync_Of_One_When_Not_Null()
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        IEnumerable<MockDataType1>? updatedResults = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync((MockDataType1?)null!, CancellationToken);
        var getNullEntities = () => updatedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        updatedResults.Should().BeEmpty();
    }

    [TestCaseShift(2)]
    [TestCase(true, true)]
    public async Task UpdateAsync_Of_One_By_Field_Value(bool isUpdate, bool doesExist)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entityToUpdate = DataSourceEntities.First();
        var updatedNameValue = entityToUpdate.Name;
        Expression<Func<MockDataType1, string?>> updateFieldExpression = entity => entity.Name;
        if (doesExist && isUpdate)
        {
            updatedNameValue = Create<string>();
            entityToUpdate.Name = updatedNameValue;
        }
        else if (!doesExist)
        {
            entityToUpdate = Create<MockDataType1>();
        }
        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async() => await MockDataSet.UpdateAsync(entityToUpdate, updateFieldExpression, updatedNameValue,CancellationToken);
        var get = () => updatedResult = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        var updatedResultShould = updatedResult.Should();
        if (doesExist)
        {
            updatedResultShould.BeEquivalentTo(entityToUpdate);
        }
        else
        {
            updatedResultShould.BeNull();
        }
    }

    [Test]
    public async Task Update_One_By_Field_Value_As_Copy()
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entityToUpdate = DataSourceEntities.First();
        var updatedNameValue = Create<string>();
        entityToUpdate.Name = updatedNameValue;
        Expression<Func<MockDataType1, string?>> updateFieldExpression = entity => entity.Name;

        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entityToUpdate, updateFieldExpression, updatedNameValue, CancellationToken);
        var get = () => updatedResult = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        updatedResult.Should().NotBeSameAs(entityToUpdate);
        updatedResult.Should().BeEquivalentTo(entityToUpdate);
    }

    [Test, Combinatorial]
    public async Task Update_One_By_Field_Value_When_Not_Null([Values(false, true)] bool isIdNull, [Values(false, true)] bool isExpressionNull, [Values(false, true)] bool isUpdateValueNull)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var originalEntity = DataSourceEntities.First();
        var id = originalEntity.Id;
        var entityToUpdate = originalEntity.DeepClone();
        var updatedNameValue = isUpdateValueNull ? null : Create<string>();
        entityToUpdate.Name = updatedNameValue;
        Expression<Func<MockDataType1, string?>> updateFieldExpression = (isExpressionNull ? null : entity => entity.Name)!;

        if (isIdNull)
        {
            entityToUpdate.Id = null!;
        }
        var expectedUpdatedResult = !isIdNull && !isExpressionNull ? entityToUpdate : originalEntity;
        MockDataType1? updatedResult = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entityToUpdate, updateFieldExpression, updatedNameValue, CancellationToken);
        var get = () => updatedResult = MockDataSource.Get<string, MockDataType1>(id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        get.Should().NotThrow();
        updatedResult.Should().BeEquivalentTo(expectedUpdatedResult);
    }

    [TestCaseRange(0, 2)]
    public async Task UpdateAsync_Of_Many(int updateCount)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var entitiesToUpdate = DataSourceEntities[..updateCount];
        IEnumerable<MockDataType1>? entitiesUpdated = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(entitiesToUpdate, CancellationToken);
        var getUpdatedEntities = () =>
        {
            entitiesUpdated = MockDataSource.Where<string, MockDataType1>(entity => entitiesToUpdate.Contains(entity));
        };

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getUpdatedEntities.Should().NotThrow();
        entitiesUpdated.Should().BeEquivalentTo(entitiesToUpdate);
    }

    [Test]
    public async Task UpdateAsync_Of_Many_When_Not_Null()
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        IEnumerable<MockDataType1>? updatedResults = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync((IEnumerable<MockDataType1>?)null!, CancellationToken);
        var getNullEntities = () => updatedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        updatedResults.Should().BeEmpty();
    }

    [TestCaseShift(2, true)]
    public async Task UpdateAsync_Of_Many_When_Data_Not_Null(bool isOneNullUpdate, bool isManyNullUpdate)
    {
        // Arrange
        MockDataSource.Add<string, MockDataType1>(DataSourceEntities);
        var nullDataToUpdate = new List<MockDataType1?>(isOneNullUpdate ? [null] : [null, null]);
        IEnumerable<MockDataType1>? updatedResults = null;

        // Act (define)
        var updateAsync = async () => await MockDataSet.UpdateAsync(nullDataToUpdate!, CancellationToken);
        var getNullEntities = () => updatedResults = MockDataSource.Where<string, MockDataType1>(data => data == null!);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        getNullEntities.Should().NotThrow();
        updatedResults.Should().BeEmpty();
    }
}