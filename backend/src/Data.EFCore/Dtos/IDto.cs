namespace OTE.Data.EFCore.Dtos;

/// <summary>Interface for all DTOs that can directly map to an entity type.</summary>
/// <typeparam name="TEntity">The corresponding entity type.</typeparam>
public interface IDto<TEntity>
{
    /// <summary>Gets a `TEntity` from a `IDto`.</summary>
    /// <returns>The generated `TEntity` instance.</summary>
    public TEntity Map();
}
