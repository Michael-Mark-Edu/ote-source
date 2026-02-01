using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing users.</summary>
[Index(nameof(Username), IsUnique = true)]
public class UserEntity
{
    [Key]
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

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

    [JsonIgnore]
    [ForeignKey("SchoolId")]
    public SchoolEntity School { get; set; } = null!;

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    [JsonPropertyName("isAdmin")]
    public bool IsAdmin { get; set; }
}
