using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing book photos.</summary>
public class BookPhotoEntity
{
    [Key]
    [JsonPropertyName("bookPhotoId")]
    public int BookPhotoId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    public BookListingEntity BookListing { get; set; } = null!;

    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    [JsonIgnore]
    public CoverPhotoEntity CoverPhoto { get; set; } = null!;

    [JsonPropertyName("coverPhotoId")]
    public int CoverPhotoId { get; set; }
}
