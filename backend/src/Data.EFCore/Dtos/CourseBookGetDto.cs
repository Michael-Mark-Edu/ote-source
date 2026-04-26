using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for course book in a GET request.</summary>
public class CourseBookGetDto : IGetDto<CourseBookEntity>
{
    [JsonPropertyName("courseBookId")]
    public int CourseBookId { get; set; }

    [JsonPropertyName("bookId")]
    public int BookId { get; set; }

    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }


    public CourseBookGetDto(CourseBookEntity courseBookEntity)
    {
        CourseId = courseBookEntity.CourseId;
        BookId = courseBookEntity.BookId;
        CourseId = courseBookEntity.CourseId;
    }

    [JsonConstructor]
    public CourseBookGetDto(int courseBookId, int bookId, int courseId)
    {
        CourseBookId = courseBookId;
        BookId = bookId;
        CourseId = courseId;
    }
}
