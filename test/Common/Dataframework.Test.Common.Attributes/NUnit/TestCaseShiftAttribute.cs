using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Queueware.Dataframework.Test.Common.Attributes.NUnit;

public sealed class TestCaseShiftAttribute : TestCaseAttribute, ITestBuilder
{
    private readonly int _columnCount;

    private readonly bool _isStartWithShiftValue;

    public TestCaseShiftAttribute(int columnCount, bool isStartWithShiftValue = false)
    {
        if (columnCount <= 0)
        {
            throw new ArgumentException($"{nameof(columnCount)} must be greater than zero." );
        }
        _columnCount = columnCount;
        _isStartWithShiftValue = isStartWithShiftValue;
    }

    public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, global::NUnit.Framework.Internal.Test? suite)
    {
        if (!_isStartWithShiftValue)
        {
            var testCaseParameters = new TestCaseParameters(Enumerable.Range(0, _columnCount).Select(_ => (object)false).ToArray());
            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, testCaseParameters);
        }
        for (var rowIndex = 0; rowIndex < _columnCount; rowIndex++)
        {
            var row = CreateRowAndPlaceShift(rowIndex);
            var testCaseParameters = new TestCaseParameters(row.Select(value => (object)value).ToArray());
            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, testCaseParameters);
        }
    }

    private bool[] CreateRowAndPlaceShift(int rowIndex)
    {
        var row = new bool[_columnCount];
        for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
        {
            if (rowIndex == columnIndex)
            {
                row[columnIndex] = true;
            }
        }

        return row;
    }
}
