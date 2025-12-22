namespace Dataframework.Abstractions.Primitives;

public interface IId<TId>
{
    TId Id { get; set; }
}