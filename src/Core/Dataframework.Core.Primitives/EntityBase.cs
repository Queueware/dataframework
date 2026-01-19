using Queueware.Dataframework.Abstractions.Primitives;

namespace Queueware.Dataframework.Core.Primitives;

/// <summary>
/// Provides a minimal base implementation for entities that require a
/// strongly-typed identifier.
/// </summary>
/// <typeparam name="TId">
/// The identifier type for the entity.
/// </typeparam>
public abstract class EntityBase<TId> : IId<TId>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public TId Id { get; set; } = default!;
}