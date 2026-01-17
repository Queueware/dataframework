using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.DataStore.Source;

internal class MockDataSourceBinder(MockDataSource mockDataSource)
{
    public MethodInfo CreateMockDataSourceBinding<TEntity>(string methodName, Func<ParameterInfo[], bool>? signatureSelector = null, params Type[] additionalTypes)
    {
        var methodInfo = signatureSelector == null
            ? GetMockDataSourceBinding(methodName)
            : GetMockDataSourceBindingFromOverloadedMethod(methodName, signatureSelector);

        if (methodInfo == null)
        {
            throw new RuntimeBinderException($"Unable to bind {nameof(TEntity)} to {methodName}");
        }

        List<Type> genericMethodArguments = [ GetEntityIdType<TEntity>(), typeof(TEntity), ..additionalTypes ];
        return methodInfo.MakeGenericMethod(genericMethodArguments.ToArray());
    }

    public MethodInfo CreateMockDataSourceWhereBinding<TEntity>() => CreateMockDataSourceBinding<TEntity>(nameof(MockDataSource.Where));

    public static object? GetEntityIdValue<TEntity>(TEntity entity)
    {
        var entityType = typeof(TEntity);
        var boundIIdInterface = entityType.GetInterfaces()
            .FirstOrDefault(type => type.Name.ToLower().StartsWith(typeof(IId<>).Name.ToLower()));
        
        if (boundIIdInterface == null)
        {
            throw new ArgumentException($"{entityType.Name} must implement {typeof(IId<>).Name}");
        }
        var idPropertyInfo = entityType.GetProperty(nameof(IId<object>.Id));

        return idPropertyInfo!.GetValue(entity);
    }

    private static Type GetEntityIdType<TEntity>()
    {
        var entityType = typeof(TEntity);

        var boundIIdInterface = entityType.GetInterfaces()
            .FirstOrDefault(type => type.Name.ToLower().StartsWith(typeof(IId<>).Name.ToLower()));
        
        if (boundIIdInterface == null)
        {
            throw new ArgumentException(
                $"{entityType.Name} must implement {typeof(IId<>).Name}.");
        }

        var boundIIdInterfaceIdType = boundIIdInterface?.GetGenericArguments().FirstOrDefault();
        if (boundIIdInterfaceIdType == null)
        {
            throw new Exception($"{entityType.Name} id type was not found.");
        }

        return boundIIdInterfaceIdType;
    }

    private MethodInfo? GetMockDataSourceBinding(string methodName) => mockDataSource.GetType().GetMethod(methodName);

    private MethodInfo? GetMockDataSourceBindingFromOverloadedMethod(string methodName, Func<ParameterInfo[], bool> signatureSelector)
    {
        return mockDataSource
            .GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(methodInfo => methodInfo.Name == methodName && methodInfo.IsGenericMethodDefinition)
            .SingleOrDefault(methodInfo => signatureSelector(methodInfo.GetParameters()));
    }
}
