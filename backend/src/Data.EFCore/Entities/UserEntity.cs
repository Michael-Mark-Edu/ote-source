using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing users.</summary>
public class UserEntity
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(255)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? MiddleName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonIgnore]
    [ForeignKey("SchoolId")]
    public SchoolEntity School { get; set; } = null!;
    public int SchoolId { get; set; }

    [JsonIgnore]
    [ForeignKey("Argon2idPasswordId")]
    public Argon2idPasswordEntity Argon2idPassword { get; set; } = null!;
    public int Argon2idPasswordId { get; set; }
}
