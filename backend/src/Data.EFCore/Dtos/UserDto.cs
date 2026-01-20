using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IDto` corresponding to `UserEntity`.</summary>
public class UserDto : IDto<UserEntity>
{
    [MaxLength(255)]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [MaxLength(255)]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; } = null;

    [MaxLength(255)]
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; } = null;

    [MaxLength(255)]
    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; } = null;

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    [JsonPropertyName("argon2idPasswordId")]
    public int Argon2idPasswordId { get; set; }

    public UserEntity Map()
    {
        return new UserEntity
        {
            UserId = 0,
            Username = Username,
            EmailAddress = EmailAddress,
            CreatedAt = CreatedAt,
            DeletedAt = DeletedAt,
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            SchoolId = SchoolId,
            Argon2idPasswordId = Argon2idPasswordId
        };
    }
}
