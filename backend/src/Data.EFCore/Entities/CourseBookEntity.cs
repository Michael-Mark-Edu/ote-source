using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing course books.</summary>
public class CourseBookEntity
{
    [Key]
    [JsonPropertyName("courseBookId")]
    public int CourseBookId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    public CourseEntity Course { get; set; } = null!;

    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }

    [JsonIgnore]
    public BookEntity ISBN { get; set; } = null!;

    [JsonPropertyName("bookId")]
    public int BookId { get; set; }
}
