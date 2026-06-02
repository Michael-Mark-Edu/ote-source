using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for returning session tokens for GET requests.</summary>
public class SessionTokenGetDto : IGetDto<SessionTokenCacheEntity>
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("token")]
    public byte[] Token { get; set; } = null!;

    public SessionTokenGetDto(SessionTokenCacheEntity sessionTokenEntity)
    {
        UserId = sessionTokenEntity.UserId;
        CreatedAt = sessionTokenEntity.CreatedAt;
        ExpiresAt = sessionTokenEntity.ExpiresAt;
        Token = sessionTokenEntity.Token;
    }

    [JsonConstructor]
    public SessionTokenGetDto(int userId, DateTime createdAt, DateTime expiresAt, byte[] token)
    {
        UserId = userId;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        Token = token;
    }
}
