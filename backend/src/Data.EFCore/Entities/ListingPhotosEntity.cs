using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing lising photos.</summary>
public class ListingPhotosEntity
{
    [Key]
    [JsonPropertyName("listingPhotoId")]
    public int ListingPhotoId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    public BookListingEntity BookListing { get; set; } = null!;

    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }
}
