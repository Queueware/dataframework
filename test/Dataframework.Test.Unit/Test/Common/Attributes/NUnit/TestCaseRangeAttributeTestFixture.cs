using NUnit.Framework.Internal;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;

namespace Queueware.Dataframework.Test.Unit.Test.Common.Attributes.NUnit;

public abstract class TestCaseRangeAttributeTestFixture : CommonTestBase
{
    protected static Func<List<List<int?>>> BuildTestCaseRangeFunc(string rangeWrapperMethodName)
    {
        var testCaseRangeAttributeTestClassType = typeof(TestCaseRangeAttributeTestClass);
        var methodInfo = new MethodWrapper(testCaseRangeAttributeTestClassType, rangeWrapperMethodName);
        var testSuite = new TestSuite(testCaseRangeAttributeTestClassType);
        var testCaseRangeAttributes = methodInfo.GetCustomAttributes<TestCaseRangeAttribute>(false);
        return () =>
        {
            return testCaseRangeAttributes.Select(attribute =>
            {
                return attribute
                    .BuildFrom(methodInfo, testSuite).Select(boundMethod => boundMethod.Arguments)
                    .SelectMany(arguments => arguments.Select(argument => argument as int?).ToList()).ToList();
            }).ToList();
        };
    }

    internal abstract class TestCaseRangeAttributeTestClass
    {
        [TestCaseRange(-2, -2)]
        [TestCaseRange(-1, -1)]
        [TestCaseRange(0, 0)]
        [TestCaseRange(1, 1)]
        [TestCaseRange(2, 2)]
        public void RangeWhenStartIsEqualToEnd(int _) {}

        [TestCaseRange(-2, -1)]
        [TestCaseRange(-2, 0)]
        [TestCaseRange(-1, 0)]
        [TestCaseRange(-1, 1)]
        [TestCaseRange(0, 1)]
        [TestCaseRange(0, 2)]
        [TestCaseRange(1, 2)]
        [TestCaseRange(1, 3)]
        [TestCaseRange(2, 3)]
        [TestCaseRange(2, 4)]
        public void RangeWhenStartIsLessThanEnd(int _) {}

        [TestCaseRange(-1, -2)]
        [TestCaseRange(0, -2)]
        [TestCaseRange(0, -1)]
        [TestCaseRange(1, -1)]
        [TestCaseRange(1, 0)]
        [TestCaseRange(2, 0)]
        [TestCaseRange(2, 1)]
        [TestCaseRange(3, 1)]
        [TestCaseRange(3, 2)]
        [TestCaseRange(4, 2)]
        public void RangeWhenStartIsGreaterThanEnd(int _) {}
    }
}