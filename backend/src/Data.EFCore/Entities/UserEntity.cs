using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing users.</summary>
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(EmailAddress), IsUnique = true)]
public class UserEntity
{
    [Key]
    public int UserId { get; set; }

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

    [JsonIgnore]
    [ForeignKey("SchoolId")]
    public SchoolEntity School { get; set; } = null!;
    public int SchoolId { get; set; }

    [JsonIgnore]
    [ForeignKey("Argon2idPasswordId")]
    public Argon2idPasswordEntity Argon2idPassword { get; set; } = null!;
    public int Argon2idPasswordId { get; set; }
}
