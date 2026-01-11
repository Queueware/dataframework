using FluentAssertions;
using NUnit.Framework;

namespace Queueware.Dataframework.Test.Unit.Test.Common.Attributes.NUnit;

[TestFixture]
public class TestCaseRangeAttributeShould : TestCaseRangeAttributeTestFixture
{
    [Test]
    public void Range_When_Equal()
    {
        // Arrange
        List<List<int>> expectedResult = [[-2], [-1], [0], [1], [2]];
        List<List<int?>>? result = null;

        // Act (define)
        var range = () => result = BuildTestCaseRangeFunc(nameof(TestCaseRangeAttributeTestClass.RangeWhenStartIsEqualToEnd))();

        // Assert
        range.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void Range_When_Start_Is_Less_Than_End()
    {
        // Arrange
        List<List<int>> expectedResult = 
        [
            [-2, -1], [-2, -1, 0],
            [-1, 0], [-1, 0, 1],
            [0, 1], [0, 1, 2],
            [1, 2], [1, 2, 3],
            [2, 3], [2, 3, 4],
        ];
        List<List<int?>>? result = null;

        // Act (define)
        var range = () => result = BuildTestCaseRangeFunc(nameof(TestCaseRangeAttributeTestClass.RangeWhenStartIsLessThanEnd))();

        // Assert
        range.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void Range_When_Start_Is_Greater_Than_End()
    {
        // Arrange
        const string ExpectedExceptionMessage = "start must be less than end.";

        // Act (define)
        var range = () => BuildTestCaseRangeFunc(nameof(TestCaseRangeAttributeTestClass.RangeWhenStartIsGreaterThanEnd))();

        // Assert
        range.Should().Throw<ArgumentException>().WithMessage(ExpectedExceptionMessage);
    }
}