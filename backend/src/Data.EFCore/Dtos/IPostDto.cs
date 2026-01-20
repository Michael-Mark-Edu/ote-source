namespace OTE.Data.EFCore.Dtos;

/// <summary>Interface for all DTOs that can safely be in POST request bodies. May be linked to multiple entities.</summary>
/// <typeparam name="TResult">The output collection/class for storing the generated entities.</typeparam>
public interface IPostDto<TResult>
{
    /// <summary>Gets all associated entities, ready to insert.</summary>
    /// <returns>The generated entities.</summary>
    public TResult Map();
}
