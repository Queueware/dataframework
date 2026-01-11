using NUnit.Framework.Internal;
using Queueware.Dataframework.Test.Common;
using Queueware.Dataframework.Test.Common.Attributes.NUnit;

namespace Queueware.Dataframework.Test.Unit.Test.Common.Attributes.NUnit;

public class TestCaseShiftAttributeTestFixture : CommonTestBase
{
    protected static Func<List<List<bool?>>> BuildTestCaseShiftFunc(string shiftWrapperMethodName)
    {
        var testCaseShiftAttributeTestClassType = typeof(TestCaseShiftAttributeTestClass);
        var methodInfo = new MethodWrapper(testCaseShiftAttributeTestClassType, shiftWrapperMethodName);
        var testSuite = new TestSuite(testCaseShiftAttributeTestClassType);
        var testCaseShiftAttribute = methodInfo.GetCustomAttributes<TestCaseShiftAttribute>(false).Single();

        return () =>
        {
            return testCaseShiftAttribute
                .BuildFrom(methodInfo, testSuite).Select(boundMethod => boundMethod.Arguments)
                .Select(arguments => arguments.Select(argument => argument as bool?).ToList())
                .ToList();
        };
    }

    internal abstract class TestCaseShiftAttributeTestClass
    {

        [TestCaseShift(-1)]
        public void ShiftLessThanZero()
        {
        }

        [TestCaseShift(-1, true)]
        public void ShiftLessThanZeroWithStartShiftValue()
        {
        }

        [TestCaseShift(0)]
        public void ShiftZero()
        {
        }

        [TestCaseShift(0, true)]
        public void ShiftZeroWithStartShiftValue()
        {
        }

        [TestCaseShift(1)]
        public void ShiftOne(bool argument1)
        {
        }


        [TestCaseShift(1, true)]
        public void ShiftOneWithStartShiftValue(bool argument1)
        {
        }

        [TestCaseShift(3)]
        public void ShiftMany(bool argument1, bool argument2, bool argument3)
        {
        }

        [TestCaseShift(3, true)]
        public void ShiftManyWithStartShiftValue(bool argument1, bool argument2, bool argument3)
        {
        }
    }
}