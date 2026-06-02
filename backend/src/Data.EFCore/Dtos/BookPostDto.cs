using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `BookPostDto`.</summary>
public class BookPostDtoOutput
{
    public required BookEntity BookEntity { get; set; }
}

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class BookPostDto : IPostDto<BookPostDtoOutput>
{
    [JsonPropertyName("isbn")]
    public string ISBN { get; set; } = null!;

    [MaxLength(255)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("authors")]
    public string Authors { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("publishers")]
    public string Publishers { get; set; } = string.Empty;

    [MaxLength(4000)]
    [JsonPropertyName("description")]
    public string? Description { get; set; } = null;

    [JsonPropertyName("publishDate")]
    public DateTime? PublishDate { get; set; } = null;

    public BookPostDtoOutput Map()
    {
        var bookEntity = new BookEntity
        {
            ISBN = ISBN,
            Title = Title,
            Authors = Authors,
            Publishers = Publishers,
            PublishDate = PublishDate,
            Description = Description
        };

        return new BookPostDtoOutput
        {
            BookEntity = bookEntity
        };
    }
}
