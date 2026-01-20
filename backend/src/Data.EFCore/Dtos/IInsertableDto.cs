namespace OTE.Data.EFCore.Dtos;

/// <summary>Interface for all DTOs that can directly map to an entity type (other than primary key).</summary>
/// <typeparam name="TEntity">The corresponding entity type.</typeparam>
public interface IInsertableDto<TEntity>
{
    /// <summary>Gets a `TEntity` from a `IInsertableDto`.</summary>
    /// <returns>The generated `TEntity` instance.</summary>
    public TEntity Map();
}
