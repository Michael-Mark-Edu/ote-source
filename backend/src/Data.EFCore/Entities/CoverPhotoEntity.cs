using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing cover photos.</summary>
public class CoverPhotoEntity
{
    [Key]
    [JsonPropertyName("coverPhotoId")]
    public int CoverPhotoId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    [ForeignKey("BookId")]
    public BookEntity ISBN { get; set; } = null!;

    [JsonPropertyName("bookId")]
    public int BookId { get; set; }
}
