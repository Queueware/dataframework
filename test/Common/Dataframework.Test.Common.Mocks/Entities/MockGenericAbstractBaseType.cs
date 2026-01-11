using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Test.Common.Mocks.Entities;

public abstract class MockGenericAbstractBaseType<TId> : IId<TId>, IEquatable<MockGenericAbstractBaseType<TId>>, IEqualityComparer<MockGenericAbstractBaseType<TId>>
{
    public required TId Id { get; set; }

    public string? Name { get; set; }
    
    public bool Equals(MockGenericAbstractBaseType<TId>? other)
    {
        var isEqual = false;
        if (other != null)
        {
            isEqual = EqualityComparer<TId>.Default.Equals(Id, other.Id) && Name == other.Name;
        }
        
        return isEqual;
    }

    public override bool Equals(object? other)
    {
        return Equals(other as MockGenericDataType1<TId>);
    }

    public override int GetHashCode() => HashCode.Combine(Id, Name);

    public bool Equals(MockGenericAbstractBaseType<TId>? x, MockGenericAbstractBaseType<TId>? y)
    {
        return x?.Equals(y as object) ?? false;
    }

    public int GetHashCode(MockGenericAbstractBaseType<TId> obj) => obj.GetHashCode();
}