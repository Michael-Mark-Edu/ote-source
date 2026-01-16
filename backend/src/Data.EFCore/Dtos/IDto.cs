namespace OTE.Data.EFCore.Dtos;

/// <summary>Interface for all DTOs that can directly map to an entity type.</summary>
/// <typeparam name="TEntity">The corresponding entity type.</typeparam>
public interface IDto<TEntity>
{
    /// <summary>Gets a `TEntity` from a `IDto`.</summary>
    /// <param name="references">List of lists of objects for doing Id <-> Entity linking.</param>
    /// <returns>The generated `TEntity` instance.</summary>
    public TEntity? Map(IEnumerable<IEnumerable<object>>? references);
}
