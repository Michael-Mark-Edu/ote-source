using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing courses.</summary>
public class CourseEntity
{
    [Key]
    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }

    [MaxLength(255)]
    [JsonPropertyName("courseTitle")]
    public string CourseTitle { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    [MaxLength(4)]
    [JsonPropertyName("subjectAcronym")]
    public string SubjectAcronym { get; set; } = string.Empty;

    // Level (100-199, 200-299, 300-399, 400-499)
    [MaxLength(3)]
    [JsonPropertyName("level")]
    public string Level { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    [ForeignKey("SchoolId")]
    public SchoolEntity School { get; set; } = null!;

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }
}
