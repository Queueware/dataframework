using System.Linq.Expressions;
using FluentAssertions;
using LinqKit;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Infrastructure.Infrastructure.Repositories;

[TestFixture]
public class RepositoryShould : RepositoryTestFixture
{
    [TestCaseRange(0, 2)]
    public async Task CountAsync(int expectedResult)
    {
        // Arrange
        var name = Create<string>();
        RepositoryEntities[..expectedResult].ForEach(entity =>
        {
            entity.Name = name;
            MockDataSource.Update<string, MockDataType1>(entity);
        });
        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == name;
        int? result = null;

        // Act (define)
        var countAsync = async () => result = await Repository.CountAsync(expression, CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
    }

    [TestCase(false, false)]
    [TestCase(true, false)]
    [TestCase(true, true)]
    public async Task DeleteAsync_Of_One(bool isFound, bool isDeleted)
    {
        // Arrange
        var entityToRemove = RepositoryEntities.First();
        var id = isFound ? entityToRemove.Id : Create<string>();
        MockDataContext.State.SaveChangesAsyncReturnValue = Convert.ToInt32(isDeleted);
        MockDataType1? entityPostRemove = null;
        bool? result = null;

        // Act (define)
        var deleteAsync = async () => result = await Repository.DeleteAsync(id, CancellationToken);
        var get = () => entityPostRemove = MockDataSource.Get<string, MockDataType1>(id);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(isDeleted);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(Convert.ToInt32(isFound));
        VerifyDataContextCreationAndDisposalCalls();
        get.Should().NotThrow();
        entityPostRemove.Should().BeNull();
    }

    [TestCaseShift(3)]
    public async Task DeleteAsync_Of_Many(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var ids = TestArrangementHelper.GetRangeFrom(RepositoryEntities, isOne, isMany, isAll).Select(entity => entity.Id);
        var shouldSaveChanges = isOne || isMany || isAll;
        var saveChangeCount = Convert.ToInt32(shouldSaveChanges);
        MockDataContext.State.SaveChangesAsyncReturnValue = saveChangeCount;
        IEnumerable<MockDataType1>? entitiesPostRemove = null;
        bool? result = null;

        // Act (define)
        var deleteAsync = async () => result = await Repository.DeleteAsync(ids, CancellationToken);
        var where = () => entitiesPostRemove = MockDataSource.Where<string, MockDataType1>(entity => ids.Contains(entity.Id));

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(shouldSaveChanges);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(saveChangeCount);
        VerifyDataContextCreationAndDisposalCalls();
        where.Should().NotThrow();
        entitiesPostRemove.Should().BeEmpty();
    }

    [TestCaseShift(3)]
    public async Task DeleteAsync_Of_Many_When_Found(bool isOneFound, bool isManyFound, bool isAllFound)
    {
        // Arrange
        var entitiesFound = TestArrangementHelper.GetRangeFrom(RepositoryEntities, isOneFound, isManyFound, isAllFound);
        var entitiesNotFound = RepositoryEntities.ExceptBy(entitiesFound.Select(entity => entity.Id), entity => entity.Id);
        MockDataSource.Remove<string, MockDataType1>(entitiesNotFound);
        var ids = RepositoryEntities.Select(entity => entity.Id);
        MockDataContext.State.SaveChangesAsyncReturnValue = Convert.ToInt32(isAllFound);

        IEnumerable<MockDataType1>? entitiesPostRemove = null;
        bool? result = null;

        // Act (define)
        var deleteAsync = async () => result = await Repository.DeleteAsync(ids, CancellationToken);
        var where = () => entitiesPostRemove = MockDataSource.Where<string, MockDataType1>(entity => ids.Contains(entity.Id));

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(isAllFound);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(Convert.ToInt32(isOneFound || isManyFound || isAllFound));
        VerifyDataContextCreationAndDisposalCalls();
        where.Should().NotThrow();
        entitiesPostRemove.Should().BeEmpty();
    }

    [TestCaseShift(3)]
    public async Task Get_FirstOrDefaultAsync(bool doesOneMatch, bool doManyMatch, bool doAllMatch)
    {
        // Arrange
        var expectedEntitiesToMatch  = TestArrangementHelper.GetRangeFrom(RepositoryEntities, doesOneMatch, doManyMatch, doAllMatch).ToList();
        var matchingName = Create<string>();
        expectedEntitiesToMatch.ForEach(entity => entity.Name = matchingName);
        MockDataSource.Update<string, MockDataType1>(expectedEntitiesToMatch);
        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == matchingName;

        var expectedResult = expectedEntitiesToMatch.FirstOrDefault();
        MockDataType1? result = null;

        // Act (define)
        var firstOrDefaultAsync = async () => result = await Repository.FirstOrDefaultAsync(expression, CancellationToken);

        // Assert
        await firstOrDefaultAsync.Should().NotThrowAsync();
        var resultShould = result.Should();
        if (expectedResult != null)
        {
            resultShould.NotBeSameAs(expectedResult);
        }
        resultShould.BeEquivalentTo(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task InsertAsync_Of_One(bool doesExist)
    {
        // Arrange
        var entityToInsert = RepositoryEntities.First();
        if (!doesExist)
        {
            MockDataSource.Remove<string, MockDataType1>(entityToInsert);
            MockDataContext.State.SaveChangesAsyncReturnValue = 1;
        }
        var expectedSaveChangesAsyncCallCount = doesExist ? 0 : 1;
        List<MockDataType1>? entitiesPostInsert = null;
        var expectedResult = MockDataContext.State.SaveChangesAsyncReturnValue > 0;
        bool? result = null;

        // Act (define)
        var insertAsync = async () => result = await Repository.InsertAsync(entityToInsert, CancellationToken);
        var where = () => entitiesPostInsert = MockDataSource.Where<string, MockDataType1>(entity => entityToInsert.Id == entity.Id).ToList();

        // Assert
        await insertAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(expectedSaveChangesAsyncCallCount);
        VerifyDataContextCreationAndDisposalCalls();
        where.Should().NotThrow();
        entitiesPostInsert.Should().NotBeNull();
        entitiesPostInsert.Count.Should().Be(1);
        entitiesPostInsert.Should().BeEquivalentTo([entityToInsert]);
    }

    [TestCaseShift(3)]
    public async Task InsertAsync_Of_Many(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var newEntities = CreateMany<MockDataType1>();
        var entitiesToInsert = TestArrangementHelper.GetRangeFrom(newEntities, isOne, isMany, isAll).ToList();
        var nonEmptyInsertCount = Convert.ToInt32(isOne || isMany || isAll);
        MockDataContext.State.SaveChangesAsyncReturnValue = nonEmptyInsertCount;

        List<MockDataType1>? entitiesPostInsert = null;
        var expectedResult = MockDataContext.State.SaveChangesAsyncReturnValue > 0;
        bool? result = null;

        // Act (define)
        var insertAsync = async () => result = await Repository.InsertAsync(entitiesToInsert, CancellationToken);
        var where = () => entitiesPostInsert = MockDataSource.Where<string, MockDataType1>(entitiesToInsert.Contains).ToList();

        // Assert
        await insertAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(nonEmptyInsertCount);
        VerifyDataContextCreationAndDisposalCalls();
        where.Should().NotThrow();
        entitiesPostInsert.Should().NotBeNull();
        if (!expectedResult)
        {
            entitiesPostInsert.Should().BeEmpty();
        }
        else
        {
            entitiesPostInsert.Should().NotBeSameAs(entitiesToInsert);
            entitiesPostInsert.Should().BeEquivalentTo(entitiesPostInsert);
        }
    }

    [TestCaseShift(3)]
    public async Task InsertAsync_Of_Many_For_Non_Existing(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var existingEntities = TestArrangementHelper.GetRangeFrom(RepositoryEntities, isOne, isMany, isAll).ToList();
        var newEntities = CreateMany<MockDataType1>().ToList();
        List<MockDataType1> entitiesToInsert = [..newEntities ];
        Enumerable.Range(0, existingEntities.Count).ForEach(index => entitiesToInsert[index] = existingEntities[index]);

        var expectedSaveChangesAsyncCallCount = Convert.ToInt32(!(isOne || isMany || isAll));
        MockDataContext.State.SaveChangesAsyncReturnValue = expectedSaveChangesAsyncCallCount;

        var expectedEntitiesPostInsert = entitiesToInsert
            .ExceptBy(existingEntities.Select(existingEntity => existingEntity.Id), entity => entity.Id)
            .ToList();
        List<MockDataType1>? entitiesPostInsert = null;
        var expectedResult = MockDataContext.State.SaveChangesAsyncReturnValue > 0;
        bool? result = null;

        // Act (define)
        var insertAsync = async () => result = await Repository.InsertAsync(entitiesToInsert, CancellationToken);
        var where = () => entitiesPostInsert = MockDataSource.Where<string, MockDataType1>(newEntities.Contains).ToList();

        // Assert
        await insertAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(Convert.ToInt32(expectedEntitiesPostInsert.Count > 0));
        VerifyDataContextCreationAndDisposalCalls();
        where.Should().NotThrow();
        entitiesPostInsert.Should().NotBeNull();
        entitiesPostInsert.Should().NotBeSameAs(expectedEntitiesPostInsert);
        entitiesPostInsert.Should().BeEquivalentTo(expectedEntitiesPostInsert);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task GetByIdAsync_Of_One_By_Id(bool doesExist)
    {
        // Arrange
        var entity = RepositoryEntities.First();
        var id = doesExist ? entity.Id : Create<string>();
        var expectedResult = doesExist ? entity : null;
        MockDataType1? result = null;

        // Act (define)
        var getByIdAsync = async () => result = await Repository.GetByIdAsync(id, CancellationToken);

        // Assert
        await getByIdAsync.Should().NotThrowAsync();
        var resultShould = result.Should();
        if (expectedResult != null)
        {
            resultShould.NotBeSameAs(expectedResult);
        }
        resultShould.BeEquivalentTo(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
    }


    [TestCaseShift(3)]
    public async Task GetByIdAsync_Of_Many_By_Ids(bool doesOneExist, bool doManyExist, bool doAllExist)
    {
        // Arrange
        var existingEntities = RepositoryEntities;
        var entitiesForRead = CreateMany<MockDataType1>().ToList();
        Enumerable.Range(0, doesOneExist ? 1 : doManyExist ? 2 : doAllExist ? 3 : 0).ForEach(index =>
        {
            entitiesForRead[index] = existingEntities[index];
        });
        var ids = entitiesForRead.Select(entity => entity.Id);

        var expectedResult = entitiesForRead.Where(existingEntities.Contains).ToList();
        List<MockDataType1>? result = null;

        // Act (define)
        var getByIdAsync = async () => result = (await Repository.GetByIdAsync(ids, CancellationToken)).ToList();

        // Assert
        await getByIdAsync.Should().NotThrowAsync();
        result.Should().NotBeSameAs(expectedResult);
        result.Should().BeEquivalentTo(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
    }

    [TestCaseShift(3)]
    public async Task FindAsync_Of_Many_By_Expression(bool doesOneMatch, bool doManyMatch, bool doAllMatch)
    {
        // Arrange
        var expectedEntitiesToMatch = TestArrangementHelper.GetRangeFrom(RepositoryEntities, doesOneMatch, doManyMatch, doAllMatch).ToList();
        var matchingName = Create<string>();
        expectedEntitiesToMatch.ForEach(entity => entity.Name = matchingName);
        MockDataSource.Update<string, MockDataType1>(expectedEntitiesToMatch);
        Expression<Func<MockDataType1, bool>> expression = entity => entity.Name == matchingName;

        List<MockDataType1>? result = null;

        // Act (define)
        var readOrDefaultAsync = async () => result = (await Repository.FindAsync(expression, CancellationToken)).ToList();

        // Assert
        await readOrDefaultAsync.Should().NotThrowAsync();
        result.Should().NotBeSameAs(expectedEntitiesToMatch);
        result.Should().BeEquivalentTo(expectedEntitiesToMatch);
        VerifyDataContextCreationAndDisposalCalls();
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task UpdateAsync_Of_One(bool doesExist)
    {
        // Arrange
        var entity = RepositoryEntities.First();
        entity.Name = Create<string>();
        var entityToUpdate = doesExist ? entity : Create<MockDataType1>();
        MockDataContext.State.SaveChangesAsyncReturnValue = Convert.ToInt32(doesExist);

        MockDataType1? entityPostUpdate = null;
        var expectedResult = MockDataContext.State.SaveChangesAsyncReturnValue > 0;
        bool? result = null;

        // Act (define)
        var updateAsync = async () => result = await Repository.UpdateAsync(entityToUpdate, CancellationToken);
        var get = () => entityPostUpdate = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
        get.Should().NotThrow();
        var entityPostUpdateShould = entityPostUpdate.Should();
        if (doesExist)
        {
            entityPostUpdateShould.NotBeSameAs(entityToUpdate);
            entityPostUpdateShould.BeEquivalentTo(entityToUpdate);
        }
        else
        {
            entityPostUpdateShould.BeNull();
        }
    }

    [TestCaseShift(3)]
    public async Task UpdateAsync_Of_Many(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var entitiesToUpdate = TestArrangementHelper.GetRangeFrom(RepositoryEntities, isOne, isMany, isAll).ToList();
        entitiesToUpdate.ForEach(entity => entity.Name = Create<string>());
        MockDataContext.State.SaveChangesAsyncReturnValue = Convert.ToInt32(isOne || isMany || isAll);

        var expectedEntitiesPostUpdate = RepositoryEntities.OrderBy(entity => entity.Id).ToList();
        List<MockDataType1>? entitiesPostUpdate = null;
        var expectedResult = MockDataContext.State.SaveChangesAsyncReturnValue > 0;
        bool? result = null;

        // Act (define)
        var updateAsync = async () => result = await Repository.UpdateAsync(entitiesToUpdate, CancellationToken);
        var where = () => entitiesPostUpdate = MockDataSource.Where<string, MockDataType1>(entity =>
        {
            return RepositoryEntities.Select(repositoryEntity => repositoryEntity.Id).Contains(entity.Id);
        }).OrderBy(entity => entity.Id).ToList();

        // Assert
        await updateAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(Convert.ToInt32(expectedResult));
        where.Should().NotThrow();
        entitiesPostUpdate.Should().NotBeSameAs(expectedEntitiesPostUpdate);
        entitiesPostUpdate.Should().BeEquivalentTo(expectedEntitiesPostUpdate);
    }

    [TestCaseShift(3)]
    public async Task UpdateAsync_Of_Many_When_Found(bool doesOneExist, bool doManyExist, bool doAllExist)
    {
        // Arrange
        List<MockDataType1> existingEntities = [..RepositoryEntities];
        var entitiesToUpdate = CreateMany<MockDataType1>().ToList();
        Enumerable.Range(0, doesOneExist ? 1 : doManyExist ? existingEntities.Count / 2 : doAllExist ? existingEntities.Count : 0)
            .ForEach(index => entitiesToUpdate[index] = existingEntities[index]);
        entitiesToUpdate.ForEach(entity => entity.Name = Create<string>());

        MockDataContext.State.SaveChangesAsyncReturnValue = Convert.ToInt32(doAllExist);

        var existingEntityIds = existingEntities.Select(existingEntity => existingEntity.Id).ToList();
        var expectedEntitiesPostUpdate = entitiesToUpdate
            .Where(entityToUpdate => existingEntityIds.Contains(entityToUpdate.Id))
            .OrderBy(entity => entity.Id)
            .ToList();
        List<MockDataType1>? entitiesPostUpdate = null;
        bool? result = null;

        // Act (define)
        var updateAsync = async () => result = await Repository.UpdateAsync(entitiesToUpdate, CancellationToken);
        var where = () => entitiesPostUpdate = MockDataSource.Where<string, MockDataType1>(entity =>
        {
            return entitiesToUpdate.Select(entityToUpdate => entityToUpdate.Id).Contains(entity.Id);
        }).OrderBy(entity => entity.Id).ToList();

        // Assert
        await updateAsync.Should().NotThrowAsync();
        result.Should().Be(doAllExist);
        VerifyDataContextCreationAndDisposalCalls();
        where.Should().NotThrow();
        entitiesPostUpdate.Should().NotBeSameAs(expectedEntitiesPostUpdate);
        entitiesPostUpdate.Should().BeEquivalentTo(expectedEntitiesPostUpdate);
        MockDataContext.State.SaveChangesAsyncCallCount.Should().Be(Convert.ToInt32(expectedEntitiesPostUpdate.Count > 0));
    }


    [TestCase(false)]
    [TestCase(true)]
    public async Task UpdateAsync_Of_One_By_Field_Value(bool doesExist)
    {
        // Arrange
        var originalEntity = RepositoryEntities.First();
        var updatedNameValue = Create<string>();
        originalEntity.Name = updatedNameValue;
        var entityToUpdate = doesExist ? originalEntity : Create<MockDataType1>();
        Expression<Func<MockDataType1, string?>> updateNameExpression = entity => entity.Name;
        MockDataContext.State.SaveChangesAsyncReturnValue = Convert.ToInt32(doesExist);

        MockDataType1? entityPostUpdate = null;
        var expectedResult = MockDataContext.State.SaveChangesAsyncReturnValue > 0;
        bool? result = null;

        // Act (define)
        var updateAsync = async () => result = await Repository.UpdateAsync(entityToUpdate, updateNameExpression, updatedNameValue, CancellationToken);
        var get = () => entityPostUpdate = MockDataSource.Get<string, MockDataType1>(entityToUpdate.Id);

        // Assert
        await updateAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        VerifyDataContextCreationAndDisposalCalls();
        get.Should().NotThrow();
        var entityPostUpdateShould = entityPostUpdate.Should();
        if (doesExist)
        {
            entityPostUpdateShould.NotBeSameAs(entityToUpdate);
            entityPostUpdateShould.BeEquivalentTo(entityToUpdate);
        }
        else
        {
            entityPostUpdateShould.BeNull();
        }
    }
}
