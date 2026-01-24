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
        var firstOrDefaultAsync = async () =>
            result = await EntityService.FirstOrDefaultAsync(expression, CancellationToken);

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
        var findAsync = async () =>
            result = (await EntityService.FindAsync(FindExpression, CancellationToken)).ToList();

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
        var expectedResult = TestArrangementHelper.GetRangeFrom(Entities, doesOneExist, doManyExist, doAllExist)
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
            MockRepository.Verify(repository => repository.GetByIdAsync(expectedIdsArgument, CancellationToken),
                Times.Once);
        }

        MockRepository.VerifyNoOtherCalls();
    }
}