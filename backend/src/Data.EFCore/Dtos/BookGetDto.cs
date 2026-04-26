using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for returning books for a GET request.</summary>
public class BookGetDto : IGetDto<BookEntity>
{
    [JsonPropertyName("isbn")]
    public string ISBN { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("authors")]
    public string Authors { get; set; } = string.Empty;

    [JsonPropertyName("publishers")]
    public string Publishers { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; } = null;

    [JsonPropertyName("publishDate")]
    public DateTime? PublishDate { get; set; } = null;

    public BookGetDto(BookEntity bookEntity)
    {
        ISBN = bookEntity.ISBN;
        Title = bookEntity.Title;
        Authors = bookEntity.Authors;
        Publishers = bookEntity.Publishers;
        PublishDate = bookEntity.PublishDate;
        Description = bookEntity.Description;
    }

    [JsonConstructor]
    public BookGetDto(string isbn, string title, string authors, string publishers, DateTime? publishDate, string? description)
    {
        ISBN = isbn;
        Title = title;
        Authors = authors;
        Publishers = publishers;
        PublishDate = publishDate;
        Description = description;
    }
}
