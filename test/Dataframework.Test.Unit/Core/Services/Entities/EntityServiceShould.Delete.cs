using FluentAssertions;
using LinqKit;
using Moq;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Services.Entities;

public partial class EntityServiceShould
{
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

}