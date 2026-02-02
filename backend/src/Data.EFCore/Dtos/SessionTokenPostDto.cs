using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IPostDto` for inserting user/password pairs from a POST request.</summary>
public class SessionTokenPostDto : IPostDto<SessionTokenCacheEntity>
{
    [MaxLength(255)]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    public SessionTokenCacheEntity Map()
    {
        return new SessionTokenCacheEntity();
    }
}
