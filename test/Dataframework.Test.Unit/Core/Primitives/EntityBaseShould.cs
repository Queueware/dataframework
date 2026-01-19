using FluentAssertions;
using NUnit.Framework;
using Queueware.Dataframework.Abstractions.Primitives;
using Queueware.Dataframework.Core.Primitives;
using Queueware.Dataframework.Test.Common;

namespace Queueware.Dataframework.Test.Unit.Core.Primitives;

public class EntityBaseShould : CommonTestBase
{
    [Test]
    public void Implement_IId()
    {
        // Arrange
        var genericEntityBaseType = typeof(EntityBase<>);
        Type? genericIIdTypeResult = null;
        Type? genericIIdTypeGenericArgumentResult = null;
        
        // Act (define)
        var getGenericIIdTypeFromGenericEntityBaseType = () =>
        {
            genericIIdTypeResult = genericEntityBaseType
                .GetInterfaces()
                .FirstOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IId<>));
        };
        var getGenericIIdTypeGenericArgumentTypeResult = () =>
        {
            genericIIdTypeGenericArgumentResult = genericIIdTypeResult?.GetGenericArguments().FirstOrDefault();
        };
        
        // Assert
        getGenericIIdTypeFromGenericEntityBaseType.Should().NotThrow();
        getGenericIIdTypeGenericArgumentTypeResult.Should().NotThrow();
        genericIIdTypeGenericArgumentResult.Should().NotBeNull();
        genericIIdTypeGenericArgumentResult.IsGenericParameter.Should().BeTrue();
    }

    [Test]
    public void Be_Abstract()
    {
        // Arrange
        // Act
        // Assert
        typeof(EntityBase<>).Should().BeAbstract();
    }
}