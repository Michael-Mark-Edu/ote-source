using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `CourseBookPostDto`.</summary>
public class CourseBookPostDtoOutput
{
    public required CourseBookEntity CourseBookEntity { get; set; }
}

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class CourseBookPostDto : IPostDto<CourseBookPostDtoOutput>
{
    [JsonPropertyName("courseId")]
    public int CourseBookId { get; set; }

    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }

    [JsonPropertyName("bookId")]
    public int BookId { get; set; }


    public CourseBookPostDtoOutput Map()
    {
        var courseBookEntity = new CourseBookEntity
        {
            CourseBookId = 0,
            CourseId = 0,
            BookId = 0
        };

        return new CourseBookPostDtoOutput
        {
            CourseBookEntity = courseBookEntity
        };
    }
}
