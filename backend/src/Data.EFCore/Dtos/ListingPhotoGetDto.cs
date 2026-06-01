using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for listing photos in a GET request.</summary>
public class ListingPhotoGetDto : IGetDto<ListingPhotoEntity>
{
    [JsonPropertyName("listingPhotoId")]
    public int ListingPhotoId { get; set; }

    [MaxLength(255)]
    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }


    public ListingPhotoGetDto(ListingPhotoEntity listingPhotoEntity)
    {
        ListingPhotoId = listingPhotoEntity.ListingPhotoId;
        BookListingId = listingPhotoEntity.BookListingId;
    }

    [JsonConstructor]
    public ListingPhotoGetDto(int listingPhotoId, int bookListingId)
    {
        ListingPhotoId = listingPhotoId;
        BookListingId = bookListingId;
    }
}
