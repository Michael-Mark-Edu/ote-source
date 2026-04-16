using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `BookPostDto`.</summary>
public class CoursePostDtoOutput
{
    public required CourseEntity CourseEntity { get; set; }
}

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class CoursePostDto : IPostDto<CoursePostDtoOutput>
{
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

    [JsonIgnore]
    [ForeignKey("SchoolId")]
    public SchoolEntity School { get; set; } = null!;

    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    public CoursePostDtoOutput Map()
    {
        var courseEntity = new CourseEntity
        {
            CourseId = 0,
            CourseTitle = CourseTitle,
            Subject = Subject,
            SubjectAcronym = SubjectAcronym,
            Level = Level,
            SchoolId = SchoolId
        };

        return new CoursePostDtoOutput
        {
            CourseEntity = courseEntity
        };
    }
}
