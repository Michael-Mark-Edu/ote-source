using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for returning books for a GET request.</summary>
public class BookGetDto : IGetDto<BookEntity>
{
    [JsonPropertyName("bookId")]
    public int BookId { get; set; }

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

    public BookGetDto(BookEntity bookEntity)
    {
        BookId = bookEntity.BookId;
        Title = bookEntity.Title;
        Authors = bookEntity.Authors;
        Publishers = bookEntity.Publishers;
        PublishDate = bookEntity.PublishDate; 
        Description = bookEntity.Description;
    }

    [JsonConstructor]
    public BookGetDto(int bookId, string title, string authors, string publishers, DateTime? publishDate, string? description)
    {
        BookId = bookId;
        Title = title;
        Authors = authors;
        Publishers= publishers;
        PublishDate = publishDate;
        Description= description;
    }
}
