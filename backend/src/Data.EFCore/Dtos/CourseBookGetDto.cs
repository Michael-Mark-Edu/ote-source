using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for courses in a GET request.</summary>
public class CourseGetDto : IGetDto<CourseEntity>
{
    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }

    [MaxLength(255)]
    [JsonPropertyName("courseTitle")]
    public string? CourseTitle { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("subject")]
    public string? Subject { get; set; } = string.Empty;

    [MaxLength(4)]
    [JsonPropertyName("subjectAcronym")]
    public string? SubjectAcronym { get; set; } = string.Empty;

    // Level (100-199, 200-299, 300-399, 400-499)
    [MaxLength(3)]
    [JsonPropertyName("level")]
    public string? Level { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    [ForeignKey("SchoolId")]
    public SchoolEntity School { get; set; } = null!;

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    public CourseGetDto(CourseEntity courseEntity)
    {
        CourseId = courseEntity.CourseId;
        CourseTitle = courseEntity.CourseTitle;
        Subject = courseEntity.Subject;
        SubjectAcronym = courseEntity.SubjectAcronym;
        Level = courseEntity.Level;
        SchoolId = courseEntity.SchoolId;
    }

    [JsonConstructor]
    public CourseGetDto(int courseId, string? courseTitle, string? subject, 
        string? subjectAcronym, string? level, int schoolId)
    {
        CourseId = courseId;
        CourseTitle = courseTitle;
        Subject = subject;
        SubjectAcronym = subjectAcronym;
        Level = level;
        SchoolId = schoolId;
    }
}
