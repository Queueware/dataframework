namespace Queueware.Dataframework.Abstractions.DataSources;

/// <summary>
/// Defines a factory responsible for creating instances of
/// <see cref="IDataContext"/>.
/// </summary>
/// <typeparam name="TDataContext">
/// The concrete data context type produced by the factory.
/// </typeparam>
public interface IDataContextFactory<out TDataContext> where TDataContext : class, IDataContext
{
    /// <summary>
    /// Creates a new data context instance.
    /// </summary>
    /// <remarks>
    /// Each invocation should return a new, independent data context
    /// instance suitable for a single unit of work.
    /// </remarks>
    /// <returns>
    /// A newly created <typeparamref name="TDataContext"/>.
    /// </returns>
    TDataContext CreateDataContext();
}