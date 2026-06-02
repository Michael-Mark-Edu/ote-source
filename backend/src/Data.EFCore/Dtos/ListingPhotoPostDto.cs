using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class ListingPhotoPostDto : IPostDto<ListingPhotoEntity>
{
    [JsonPropertyName("photoIndex")]
    public int PhotoIndex { get; set; }

    [JsonPropertyName("photoData")]
    public string PhotoData { get; set; } = null!;

    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    public ListingPhotoEntity Map()
    {
        return new ListingPhotoEntity
        {
            ListingPhotoId = 0,
            PhotoIndex = PhotoIndex,
            PhotoUrl = "",
            CreatedAt = DateTime.UtcNow,
            DeletedAt = null,
            BookListingId = BookListingId,
        };
    }
}
