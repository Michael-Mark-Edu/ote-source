namespace OTE.Data.EFCore.Dtos;

/// <summary>Interface for all DTOs that can safely be in GET response bodies. May be linked to multiple entities.</summary>
/// <typeparam name="TBase">The input collection/class that the `IGetDto` can be constructed from.</typeparam>
public interface IGetDto<TBase>
{}
