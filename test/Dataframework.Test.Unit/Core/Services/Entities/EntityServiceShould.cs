using System.Linq.Expressions;
using FluentAssertions;
using LinqKit;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Services.Entities;

public class EntityServiceShould : EntityServiceTestFixture
{
    [TestCaseRange(0, 2)]
    public async Task CountAsync(int expectedResult)
    {
        // Arrange
        int? result = null;
        MockRepository
            .Setup(repository => repository.CountAsync(CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var countAsync = async () => result = await EntityService.CountAsync(CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        MockRepository.Verify(repository => repository.CountAsync(CancellationToken), Times.Once);
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseRange(0, 2)]
    public async Task CountAsync_By_Expression(int expectedResult)
    {
        // Arrange
        int? result = null;
        MockRepository
            .Setup(repository => repository.CountAsync(FindExpression, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var countAsync = async () => result = await EntityService.CountAsync(FindExpression, CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        MockRepository.Verify(repository => repository.CountAsync(FindExpression, CancellationToken), Times.Once);
        MockRepository.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task CountAsync_By_Expression_With_Null_Guard()
    {
        // Arrange
        const int ExpectedResult = 0;
        int? result = null;

        // Act (define)
        var countAsync = async () => result = await EntityService.CountAsync(null!, CancellationToken);

        // Assert
        await countAsync.Should().NotThrowAsync();
        result.Should().Be(ExpectedResult);
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCaseShift(2)]
    public async Task DeleteAsync_Of_One_Id(bool isDeleted, bool isNull)
    {
        // Arrange
        var id = isNull ? null! : Entities.First().Id;
        var expectedResult = isDeleted && !isNull;
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.DeleteAsync(It.IsAny<string>(), CancellationToken))
            .ReturnsAsync(isDeleted);

        // Act (define)
        var deleteAsync = async () => result = await EntityService.DeleteAsync(id, CancellationToken);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isNull)
        {
            MockRepository.Verify(repository => repository.DeleteAsync(id, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task DeleteAsync_Of_Many_Ids(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var ids = TestArrangementHelper.GetRangeFrom(Entities, isOne, isMany, isAll).Select(entity => entity.Id);
        var isEmpty = !(isOne || isMany || isAll);
        var expectedResult = !isEmpty;
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.DeleteAsync(ids, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var deleteAsync = async () => result = await EntityService.DeleteAsync(ids, CancellationToken);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isEmpty)
        {
            MockRepository.Verify(repository => repository.DeleteAsync(ids, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task DeleteAsync_Of_Many_Ids_With_Null_Guard(bool isOneNull, bool areManyNull, bool areAllNull)
    {
        // Arrange
        var ids = Entities.Select(entity => entity.Id).ToList();
        var maxIndex = isOneNull ? 1 : areManyNull ? ids.Count / 2 : areAllNull ? ids.Count : 0;
        Enumerable.Range(0, maxIndex).ForEach(index => ids[index] = null!);
        var expectedIdsArgument = ids.Where(id => id != null!);

        var expectedResult = !areAllNull;
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.DeleteAsync(expectedIdsArgument, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var deleteAsync = async () => result = await EntityService.DeleteAsync(ids, CancellationToken);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            MockRepository.Verify(repository => repository.DeleteAsync(expectedIdsArgument, CancellationToken),
                Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(2)]
    public async Task DeleteAsync_Of_One_Entity(bool isDeleted, bool isNull)
    {
        // Arrange
        var entityToRemove = isNull ? null! : Entities.First();
        var expectedResult = isDeleted && !isNull;
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.DeleteAsync(entityToRemove, CancellationToken))
            .ReturnsAsync(isDeleted);

        // Act (define)
        var deleteAsync = async () => result = await EntityService.DeleteAsync(entityToRemove, CancellationToken);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isNull)
        {
            MockRepository.Verify(repository => repository.DeleteAsync(entityToRemove, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task DeleteAsync_Of_Many_Entities(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var entitiesToRemove = TestArrangementHelper.GetRangeFrom(Entities, isOne, isMany, isAll).ToList();
        var isEmpty = !(isOne || isMany || isAll);
        var expectedResult = !isEmpty;
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.DeleteAsync(entitiesToRemove, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var deleteAsync = async () => result = await EntityService.DeleteAsync(entitiesToRemove, CancellationToken);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isEmpty)
        {
            MockRepository.Verify(repository => repository.DeleteAsync(entitiesToRemove, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task DeleteAsync_Of_Many_Entities_With_Null_Guard(bool isOneNull, bool areManyNull, bool areAllNull)
    {
        // Arrange
        List<MockDataType1> entitiesToRemove = [..Entities];
        var maxIndex = isOneNull ? 1 : areManyNull ? entitiesToRemove.Count / 2 : areAllNull ? entitiesToRemove.Count : 0;
        Enumerable.Range(0, maxIndex).ForEach(index => entitiesToRemove[index] = null!);
        var expectedEntitiesArgument = entitiesToRemove.Where(id => id != null!);

        var expectedResult = !areAllNull;
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.DeleteAsync(expectedEntitiesArgument, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var deleteAsync = async () => result = await EntityService.DeleteAsync(entitiesToRemove, CancellationToken);

        // Assert
        await deleteAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            MockRepository.Verify(repository => repository.DeleteAsync(expectedEntitiesArgument, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
        
    }
    
    [TestCaseShift(2)]
    public async Task Get_FirstOrDefaultAsync(bool isFound, bool isExpressionNull)
    {
        // Arrange
        var entityFound = isFound ? Entities.First() : null;
        var expression = isExpressionNull ? null! : FindExpression;
        
        MockRepository
            .Setup(repository => repository.FirstOrDefaultAsync(expression, CancellationToken))
            .ReturnsAsync(entityFound);

        var expectedResult = isFound && !isExpressionNull ? entityFound : null;
        MockDataType1? result = null;

        // Act (define)
        var firstOrDefaultAsync = async () => result = await EntityService.FirstOrDefaultAsync(expression, CancellationToken);

        // Assert
        await firstOrDefaultAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isExpressionNull)
        {
            MockRepository.Verify(repository => repository.FirstOrDefaultAsync(expression, CancellationToken),
                Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task FindAsync(bool doesOneMatch, bool doManyMatch, bool doAllMatch)
    {
        // Arrange
        var matchedEntities = TestArrangementHelper.GetRangeFrom(Entities, doesOneMatch, doManyMatch, doAllMatch)
            .ToList();
        List<MockDataType1>? result = null;
        
        MockRepository
            .Setup(repository => repository.FindAsync(FindExpression, CancellationToken))
            .ReturnsAsync(matchedEntities);

        // Act (define)
        var findAsync = async () => result = (await EntityService.FindAsync(FindExpression, CancellationToken)).ToList();

        // Assert
        await findAsync.Should().NotThrowAsync();
        result.Should().BeEquivalentTo(matchedEntities);
        MockRepository.Verify(repository => repository.FindAsync(FindExpression, CancellationToken), Times.Once);
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCaseShift(2)]
    public async Task Get_Of_One_By_Id(bool doesExist, bool isIdNull)
    {
        // Arrange
        var entity = Entities.First();
        var id = isIdNull ? null! : entity.Id;
        var expectedResult = doesExist ? entity : null;
        MockDataType1? result = null;
        
        MockRepository
            .Setup(repository => repository.GetByIdAsync(id, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var getAsync = async () => result = await EntityService.GetAsync(id, CancellationToken);

        // Assert
        await getAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isIdNull)
        {
            MockRepository.Verify(repository => repository.GetByIdAsync(id, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCaseShift(3)]
    public async Task GetAsync_Of_Many_By_Ids_Based_On_Existence(bool doesOneExist, bool doManyExist, bool doAllExist)
    {
        // Arrange
        var ids = Entities.Select(entity => entity.Id);
        var expectedResult= TestArrangementHelper.GetRangeFrom(Entities, doesOneExist, doManyExist, doAllExist)
            .ToList();
        List<MockDataType1>? result = null;

        MockRepository
            .Setup(repository => repository.GetByIdAsync(ids, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var getAsync = async () => result = (await EntityService.GetAsync(ids, CancellationToken)).ToList();

        // Assert
        await getAsync.Should().NotThrowAsync();
        result.Should().BeEquivalentTo(expectedResult);
        MockRepository.Verify(repository => repository.GetByIdAsync(ids, CancellationToken), Times.Once);
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task GetAsync_Of_Many_By_Ids_Based_On_Id_Count(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var allEntityIds = Entities.Select(entity => entity.Id).ToList();
        var ids = TestArrangementHelper.GetRangeFrom(allEntityIds, isOne, isMany, isAll).ToList();
        var isEmpty = !(isOne || isMany || isAll);
        var expectedResult = Entities.Where(entity => ids.Contains(entity.Id)).ToList();
        List<MockDataType1>? result = null;
        
        MockRepository
            .Setup(repository => repository.GetByIdAsync(ids, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var getAsync = async () => result = (await EntityService.GetAsync(ids, CancellationToken)).ToList();

        // Assert
        await getAsync.Should().NotThrowAsync();
        result.Should().BeEquivalentTo(expectedResult);
        if (!isEmpty)
        {
            MockRepository.Verify(repository => repository.GetByIdAsync(ids, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(3)]
    public async Task GetAsync_Of_Many_By_Ids_With_Null_Guard(bool isOneNull, bool areManyNull, bool areAllNull)
    {
        // Arrange
        var ids = Entities.Select(entity => entity.Id).ToList();
        var maxIndex = isOneNull ? 1 : areManyNull ? ids.Count / 2 : areAllNull ? ids.Count : 0;
        Enumerable.Range(0, maxIndex).ForEach(index => ids[index] = null!);
        
        var expectedIdsArgument = ids.Where(id => id != null!).ToList();
        var expectedResult = Entities.Where(entity => expectedIdsArgument.Contains(entity.Id)).ToList();
        List<MockDataType1>? result = null;
        
        MockRepository
            .Setup(repository => repository.GetByIdAsync(expectedIdsArgument, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var getAsync = async () => result = (await EntityService.GetAsync(ids, CancellationToken)).ToList();

        // Assert
        await getAsync.Should().NotThrowAsync();
        result.Should().BeEquivalentTo(expectedResult);
        if (!areAllNull)
        {
            MockRepository.Verify(repository => repository.GetByIdAsync(expectedIdsArgument, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCase(false, false, false)]
    [TestCase(false, false, true)]
    [TestCase(true, false, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, false)]
    public async Task SaveAsync_Of_One(bool isNew, bool isNull, bool isSaved)
    {
        // Arrange
        var entity= Entities.First();
        IEnumerable<MockDataType1> entityArgument = [entity];
        entity.Id = isNew ? null! : Create<string>();
        var entityToSave = isNull ? null! : entity;
        
        MockRepository
            .Setup(repository => repository.InsertAsync(entityArgument, CancellationToken))
            .ReturnsAsync(isSaved);
        MockRepository
            .Setup(repository => repository.UpdateAsync(entityArgument, CancellationToken))
            .ReturnsAsync(isSaved);
            
        bool? result = null;

        // Act (define)
        var saveAsync = async () => result = await EntityService.SaveAsync(entityToSave!, CancellationToken);

        // Assert
        await saveAsync.Should().NotThrowAsync();
        result.Should().Be(isSaved);
        if (!isNull)
        {
            if (isNew)
            {
                MockRepository.Verify(repository => repository.InsertAsync(entityArgument, CancellationToken),
                    Times.Once);
            }
            else
            {
                MockRepository.Verify(repository => repository.UpdateAsync(entityArgument, CancellationToken),
                    Times.Once);
            }
        }
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCaseShift(3)]
    public async Task SaveAsync_Of_Many(bool isOne, bool isMany, bool isAll)
    {
        // Arrange
        var entitiesToSave = TestArrangementHelper.GetRangeFrom(Entities, isOne, isMany, isAll).ToList();
        entitiesToSave.ForEach(entity => entity.Id = null!);
        
        var isEmpty = !(isOne || isMany || isAll);
        var expectedResult = !isEmpty;
        bool? result = null;
        MockRepository
            .Setup(repository => repository.InsertAsync(entitiesToSave, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var saveAsync = async () => result = await EntityService.SaveAsync(entitiesToSave, CancellationToken);

        // Assert
        await saveAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isEmpty)
        {
            MockRepository.Verify(repository => repository.InsertAsync(entitiesToSave, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
    
    [TestCaseShift(2)]
    public async Task SaveAsync_Of_Many(bool isOneNew, bool isManyNew)
    {
        // Arrange
        List<MockDataType1> entitiesToSave = [..Entities];
        var maxIndex = isOneNew ? 1 : isManyNew ? entitiesToSave.Count : 0;
        Enumerable.Range(0, maxIndex).ForEach(index => entitiesToSave[index].Id = null!);
        var newEntities = entitiesToSave.Where(entity => entity.Id == null!).ToList();
        var updatedEntities = entitiesToSave.Where(entity => entity.Id != null!).ToList();
            
        const bool ExpectedResult = true;
        bool? result = null;
        MockRepository
            .Setup(repository => repository.InsertAsync(newEntities, CancellationToken))
            .ReturnsAsync(ExpectedResult);
        MockRepository
            .Setup(repository => repository.UpdateAsync(updatedEntities, CancellationToken))
            .ReturnsAsync(ExpectedResult);

        // Act (define)
        var saveAsync = async () => result = await EntityService.SaveAsync(entitiesToSave, CancellationToken);

        // Assert
        await saveAsync.Should().NotThrowAsync();
        result.Should().Be(ExpectedResult);
        if (newEntities.Count > 0)
        {
            MockRepository.Verify(repository => repository.InsertAsync(newEntities, CancellationToken), Times.Once);
        }
        if (updatedEntities.Count > 0)
        {
            MockRepository.Verify(repository => repository.UpdateAsync(updatedEntities, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCaseShift(2, true)]
    public async Task SaveAsync_Of_Many_With_Null_Guard(bool isMany, bool isAll)
    {
        // Arrange
        List<MockDataType1> entitiesToSave = [..Entities];
        var maxIndex = isMany ? entitiesToSave.Count / 2 : entitiesToSave.Count;
        Enumerable.Range(0, maxIndex).ForEach(index => entitiesToSave[index] = null!);
        var expectedEntitiesArgument = entitiesToSave.Where(entity => entity != null!).ToList();
        var isEmpty = expectedEntitiesArgument.Count == 0;

        var expectedResult = !isAll;
        bool? result = null;
        MockRepository
            .Setup(repository => repository.UpdateAsync(expectedEntitiesArgument, CancellationToken))
            .ReturnsAsync(expectedResult);

        // Act (define)
        var saveAsync = async () => result = await EntityService.SaveAsync(entitiesToSave, CancellationToken);

        // Assert
        await saveAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (!isEmpty)
        {
            MockRepository.Verify(repository => repository.UpdateAsync(expectedEntitiesArgument, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }

    [TestCase(false, false, false)]
    [TestCase(true, false, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, false)]
    [TestCase(false, true, true)]
    [TestCase(false, false, true)]
    public async Task SaveAsync_By_Field(bool isNew, bool isEntityNull, bool isExpressionNull)
    {
        // Arrange
        var entity = Entities.First();
        if (isNew)
        {
            entity.Id = null!;
        }
        if (isEntityNull)
        {
            entity = null!;
        }
        if (isExpressionNull)
        {
            SetStringExpression = null!;
        }
        var value = Create<string>();
        var expectedResult = !(isNew || isEntityNull || isExpressionNull);
        bool? result = null;
        
        MockRepository
            .Setup(repository => repository.UpdateAsync(entity, SetStringExpression, value, CancellationToken))
            .ReturnsAsync(expectedResult);
        
        // Act (define)
        var saveAsync = async () =>
        {
            result = await EntityService.SaveAsync(entity, SetStringExpression, value, CancellationToken);
        };

        // Assert
        await saveAsync.Should().NotThrowAsync();
        result.Should().Be(expectedResult);
        if (expectedResult)
        {
            MockRepository.Verify(repository => repository.UpdateAsync(entity, SetStringExpression, value, CancellationToken), Times.Once);
        }
        MockRepository.VerifyNoOtherCalls();
    }
}