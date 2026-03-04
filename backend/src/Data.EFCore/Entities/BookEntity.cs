using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing books.</summary>
public class BookEntity
{
    [Key]
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

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;
}
