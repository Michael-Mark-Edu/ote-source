using System.ComponentModel.DataAnnotations;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IDto` corresponding to `UserEntity`.</summary>
public class UserDto : IDto<UserEntity>
{
    [MaxLength(255)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? MiddleName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string EmailAddress { get; set; } = string.Empty;

    public int SchoolId { get; set; }

    public int Argon2idPasswordId { get; set; }

    public UserEntity? Map(IEnumerable<IEnumerable<object>>? references)
    {
        if (references == null)
            return null;

        SchoolEntity? school = null;
        Argon2idPasswordEntity? password = null;

        foreach (var list in references)
        {
            if (list == null)
                continue;

            if (list.First().GetType() == typeof(SchoolEntity))
                school = (SchoolEntity?)list.Where(e => ((SchoolEntity)e).SchoolId == SchoolId).FirstOrDefault();
            else if (list.First().GetType() == typeof(Argon2idPasswordEntity))
                password = (Argon2idPasswordEntity?)list.Where(e => ((Argon2idPasswordEntity)e).Argon2idPasswordId == Argon2idPasswordId).FirstOrDefault();
        }

        if (school == null || password == null)
            return null;

        return new UserEntity
        {
            UserId = 0,
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            EmailAddress = EmailAddress,
            School = school,
            Argon2idPassword = password
        };
    }
}
