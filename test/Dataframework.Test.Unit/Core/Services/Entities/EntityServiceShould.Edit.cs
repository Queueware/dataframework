using FluentAssertions;
using LinqKit;
using Moq;
using NUnit.Framework;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;
using Queueware.Dataframework.Test.Common.Utils;
using Queueware.Dataframework.Test.Unit.Test.Common.Mocks;

namespace Queueware.Dataframework.Test.Unit.Core.Services.Entities;

public partial class EntityServiceShould
{
    [TestCase(false, false, false)]
    [TestCase(false, false, true)]
    [TestCase(true, false, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, false)]
    public async Task SaveAsync_Of_One(bool isNew, bool isNull, bool isSaved)
    {
        // Arrange
        var entity = Entities.First();
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
            MockRepository.Verify(repository => repository.UpdateAsync(expectedEntitiesArgument, CancellationToken),
                Times.Once);
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
            MockRepository.Verify(
                repository => repository.UpdateAsync(entity, SetStringExpression, value, CancellationToken),
                Times.Once);
        }

        MockRepository.VerifyNoOtherCalls();
    }
}