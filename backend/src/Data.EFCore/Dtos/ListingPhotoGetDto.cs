using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for listing photos in a GET request.</summary>
public class ListingPhotoGetDto : IGetDto<ListingPhotoEntity>
{
    [JsonPropertyName("listingPhotoId")]
    public int ListingPhotoId { get; set; }

    [JsonPropertyName("photoIndex")]
    public int PhotoIndex { get; set; }

    [JsonPropertyName("photoUrl")]
    public string PhotoUrl { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ListingPhotoGetDto(ListingPhotoEntity listingPhotoEntity)
    {
        ListingPhotoId = listingPhotoEntity.ListingPhotoId;
        PhotoIndex = listingPhotoEntity.PhotoIndex;
        PhotoUrl = listingPhotoEntity.PhotoUrl;
        CreatedAt = listingPhotoEntity.CreatedAt;
    }

    [JsonConstructor]
    public ListingPhotoGetDto(int listingPhotoId, int photoIndex, string photoUrl, DateTime createdAt)
    {
        ListingPhotoId = listingPhotoId;
        PhotoIndex = photoIndex;
        PhotoUrl = photoUrl;
        CreatedAt = createdAt;
    }
}
