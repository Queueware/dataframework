using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Queueware.Dataframework.Test.Common.Attributes.NUnit;

public class TestCaseRangeAttribute : TestCaseAttribute, ITestBuilder
{
    private readonly int _start;

    private readonly int _end;

    public TestCaseRangeAttribute(int start, int end)
    {
        if (start > end)
        {
            throw new ArgumentException($"{nameof(start)} must be less than {nameof(end)}.");
        }

        _start = start;
        _end = end;
    }
    public new IEnumerable<TestMethod> BuildFrom(IMethodInfo method, global::NUnit.Framework.Internal.Test? suite)
    {
        for (var iterator = _start; iterator <= _end; iterator++)
        {
            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new TestCaseParameters([iterator]));
        }
    }
}
