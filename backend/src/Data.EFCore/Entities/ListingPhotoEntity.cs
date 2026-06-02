using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing a lising photo.</summary>
public class ListingPhotoEntity
{
    [Key]
    [JsonPropertyName("listingPhotoId")]
    public int ListingPhotoId { get; set; }

    [JsonPropertyName("photoIndex")]
    public int PhotoIndex { get; set; }

    [JsonPropertyName("photoUrl")]
    public string PhotoUrl { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    [ForeignKey(nameof(BookListingId))]
    public BookListingEntity BookListing { get; set; } = null!;

    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }
}
