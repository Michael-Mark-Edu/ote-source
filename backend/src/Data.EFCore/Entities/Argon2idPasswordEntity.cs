using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing password hashes.</summary>
public class Argon2idPasswordEntity
{
    [Key]
    [JsonPropertyName("argon2idPasswordId")]
    public int Argon2idPasswordId { get; set; }

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
}
