using OTE.Data.EFCore.Entities;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IDto` corresponding to `Argon2idPasswordEntity`.</summary>
public class Argon2idPasswordDto : IDto<Argon2idPasswordEntity>
{
    [JsonPropertyName("version")]
    public byte Version { get; set; }

    [JsonPropertyName("memoryCost")]
    public int MemoryCost { get; set; }

    [JsonPropertyName("iterations")]
    public int Iterations { get; set; }

    [JsonPropertyName("parallelism")]
    public byte Parallelism { get; set; }

    [JsonPropertyName("salt")]
    public byte[] Salt { get; set; } = null!;

    [JsonPropertyName("hash")]
    public byte[] Hash { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    public Argon2idPasswordEntity Map()
    {
        return new Argon2idPasswordEntity
        {
            Argon2idPasswordId = 0,
            Version = Version,
            MemoryCost = MemoryCost,
            Iterations = Iterations,
            Parallelism = Parallelism,
            Salt = Salt,
            Hash = Hash,
            CreatedAt = CreatedAt,
            DeletedAt = DeletedAt
        };
    }
}
