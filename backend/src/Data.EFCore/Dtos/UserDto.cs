using System.ComponentModel.DataAnnotations;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IDto` corresponding to `UserEntity`.</summary>
public class UserDto : IDto<UserEntity>
{
    [MaxLength(255)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(255)]
    public string EmailAddress { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(255)]
    public string? FirstName { get; set; } = null;

    [MaxLength(255)]
    public string? LastName { get; set; } = null;

    [MaxLength(255)]
    public string? MiddleName { get; set; } = null;

    public int SchoolId { get; set; }

    public int Argon2idPasswordId { get; set; }

    public UserEntity Map()
    {
        return new UserEntity
        {
            UserId = 0,
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            EmailAddress = EmailAddress,
            SchoolId = SchoolId,
            Argon2idPasswordId = Argon2idPasswordId
        };
    }
}
