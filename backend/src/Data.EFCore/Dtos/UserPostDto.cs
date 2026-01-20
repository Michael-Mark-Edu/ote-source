using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Result type for `UserPostDto`.</summary>
public class UserPostDtoResult
{
    public required UserEntity UserEntity { get; set; }
    public required Argon2idPasswordEntity Argon2idPasswordEntity { get; set; }
}

/// <summary>`IPostDto` for inserting user/password pairs from a POST request.</summary>
public class UserPostDto : IPostDto<UserPostDtoResult>
{
    [MaxLength(255)]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; } = null;

    [MaxLength(255)]
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; } = null;

    [MaxLength(255)]
    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; } = null;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    public UserPostDtoResult Map()
    {
        var argon2idPasswordEntity = new Argon2idPasswordEntity
        {
            // TODO: Implement Argon2id
            Argon2idPasswordId = 0,
            Version = 0,
            MemoryCost = 0,
            Iterations = 0,
            Parallelism = 0,
            Salt = [],
            Hash = [],
            CreatedAt = DateTime.UtcNow,
            DeletedAt = null
        };

        var userEntity = new UserEntity
        {
            UserId = 0,
            Username = Username,
            EmailAddress = EmailAddress,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = null,
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            SchoolId = SchoolId,
            Argon2idPassword = argon2idPasswordEntity,
            Argon2idPasswordId = 0
        };

        return new UserPostDtoResult
        {
            UserEntity = userEntity,
            Argon2idPasswordEntity = argon2idPasswordEntity
        };
    }
}
