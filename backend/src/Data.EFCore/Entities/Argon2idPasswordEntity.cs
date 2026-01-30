using NSec.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
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

    [JsonIgnore]
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    public Argon2idPasswordEntity() { }

    public Argon2idPasswordEntity(string password)
    {
        Argon2idPasswordId = 0;
        Version = 13;
        MemoryCost = 4096;
        Iterations = 3;
        Parallelism = 1;

        Salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(Salt);

        var parameters = new Argon2Parameters();
        parameters.DegreeOfParallelism = Parallelism;
        parameters.MemorySize = MemoryCost;
        parameters.NumberOfPasses = Iterations;

        var argon = PasswordBasedKeyDerivationAlgorithm.Argon2id(parameters);
        Hash = argon.DeriveBytes(password, Salt, 16);
    }
}
