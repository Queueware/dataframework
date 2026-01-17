using FluentAssertions;
using NUnit.Framework;

namespace Queueware.Dataframework.Test.Unit.Test.Common.Attributes.NUnit;

[TestFixture]
public class TestCaseShiftAttributeShould : TestCaseShiftAttributeTestFixture
{
    [TestCase(true, false, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, false)]
    [TestCase(false, true, true)]
    public void ShiftZeroOrLessThanZero(bool isZeroShift, bool isLessThanZeroShift, bool isStartWithShiftValue)
    {
        // Arrange
        string shiftWrapperMethodName;
        if (isStartWithShiftValue)
        {
            shiftWrapperMethodName = isZeroShift
                ? nameof(TestCaseShiftAttributeTestClass.ShiftZeroWithStartShiftValue)
                : nameof(TestCaseShiftAttributeTestClass.ShiftLessThanZeroWithStartShiftValue);
        }
        else
        {
            shiftWrapperMethodName = isZeroShift
                ? nameof(TestCaseShiftAttributeTestClass.ShiftZero)
                : nameof(TestCaseShiftAttributeTestClass.ShiftLessThanZero);
        }

        const string ExpectedExceptionMessage = "columnCount must be greater than zero.";

        // Act (define)
        var shift = () => BuildTestCaseShiftFunc(shiftWrapperMethodName);

        // Assert
        shift.Should().Throw<ArgumentException>().WithMessage(ExpectedExceptionMessage);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ShiftOne(bool isStartWithShiftValue)
    {
        // Arrange
        var shiftWrapperMethodName = isStartWithShiftValue
            ? nameof(TestCaseShiftAttributeTestClass.ShiftOneWithStartShiftValue)
            : nameof(TestCaseShiftAttributeTestClass.ShiftOne);
        List<List<bool?>> expectedResult = isStartWithShiftValue ? [[true]] : [[false], [true]];
        List<List<bool?>>? result = null;

        // Act (define)
        var shift = () => result = BuildTestCaseShiftFunc(shiftWrapperMethodName)();

        // Assert
        shift.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ShiftMany(bool isStartWithShiftValue)
    {
        // Arrange
        var shiftWrapperMethodName = isStartWithShiftValue
            ? nameof(TestCaseShiftAttributeTestClass.ShiftManyWithStartShiftValue)
            : nameof(TestCaseShiftAttributeTestClass.ShiftMany);
        List<List<bool?>> expectedResult = isStartWithShiftValue
            ? [[true, false, false], [false, true, false], [false, false, true]]
            : [[false, false, false], [true, false, false], [false, true, false], [false, false, true]];
        List<List<bool?>>? result = null;

        // Act (define)
        var shift = () => result = BuildTestCaseShiftFunc(shiftWrapperMethodName)();

        // Assert
        shift.Should().NotThrow();
        result.Should().BeEquivalentTo(expectedResult);
    }
}