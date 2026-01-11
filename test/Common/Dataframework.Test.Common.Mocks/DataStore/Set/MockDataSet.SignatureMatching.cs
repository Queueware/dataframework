using System.Linq.Expressions;
using System.Reflection;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Set;

public partial class MockDataSet<TEntity> where TEntity : class
{
    private static bool IsGenericEnumerableParameterizedSignature(ParameterInfo[] parameterInfos)
    {
        return parameterInfos.Any(parameterInfo => parameterInfo.ParameterType.IsGenericType &&
                                                   parameterInfo.ParameterType.GetGenericTypeDefinition() ==
                                                   typeof(IEnumerable<>));
    }

    private static bool IsGenericSingleParameterizedSignature(ParameterInfo[] parameterInfos)
    {
        return parameterInfos is [{ ParameterType.IsGenericMethodParameter: true }];
    }

    private static bool IsVoidParameterizedSignature(ParameterInfo[] parameterInfos)
    {
        return parameterInfos is [];
    }

    private static bool IsGenericFieldValueSignature(ParameterInfo[] parameterInfos)
    {
        var isUpdateByFieldSignatureFound = false;
        const int ParameterInfosLength = 3;
        if (parameterInfos.Length == ParameterInfosLength)
        {
            var expectedIdParameterType = parameterInfos[0].ParameterType;
            var expectedFieldParameterType = parameterInfos[1].ParameterType;
            var expectedValueParameterType = parameterInfos[2].ParameterType;

            isUpdateByFieldSignatureFound = expectedIdParameterType.IsGenericParameter &&
                                            expectedFieldParameterType.IsGenericType &&
                                            expectedFieldParameterType.GetGenericTypeDefinition() ==
                                            typeof(Expression<>) &&
                                            expectedValueParameterType.IsGenericParameter;
        }

        return isUpdateByFieldSignatureFound;
    }
}