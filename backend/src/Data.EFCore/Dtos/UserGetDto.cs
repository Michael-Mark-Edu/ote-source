using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for returning users for a GET request.</summary>
public class UserGetDto : IGetDto<UserEntity>
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

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    public UserGetDto(UserEntity userEntity)
    {
        Username = userEntity.Username;
        EmailAddress = userEntity.EmailAddress;
        FirstName = userEntity.FirstName;
        LastName = userEntity.LastName;
        MiddleName = userEntity.MiddleName;
        SchoolId = userEntity.SchoolId;
    }

    [JsonConstructor]
    public UserGetDto(string username, string emailAddress, string? firstName, string? lastName, string? middleName, int schoolId)
    {
        Username = username;
        EmailAddress = emailAddress;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        SchoolId = schoolId;
    }
}
