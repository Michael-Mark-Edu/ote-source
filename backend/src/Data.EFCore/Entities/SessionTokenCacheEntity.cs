using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing session tokens.</summary>
public class SessionTokenCacheEntity
{
    [Key]
    [JsonIgnore]
    public int SessionTokenCacheId { get; set; }

    [JsonIgnore]
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("token")]
    public byte[] Token { get; set; } = null!;

    public SessionTokenCacheEntity()
    {
        Token = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(Token);

        ExpiresAt = CreatedAt.AddDays(7);
    }
}
