namespace Dataframework.Abstractions.Primitives;

/// <summary>
/// Defines a contract for types that expose a strongly-typed identifier.
/// </summary>
/// <typeparam name="TId"> The identifier type.</typeparam>
public interface IId<TId>
{    
    /// <summary>
    /// Gets or sets the unique identifier for the instance.
    /// </summary>
    TId Id { get; set; }
}