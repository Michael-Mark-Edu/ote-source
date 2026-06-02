using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for listing photos in a GET request.</summary>
public class ListingPhotosGetDto : IGetDto<ListingPhotosEntity>
{
    [JsonPropertyName("listingPhotoId")]
    public int ListingPhotoId { get; set; }

    [MaxLength(255)]
    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }


    public ListingPhotosGetDto(ListingPhotosEntity listingPhotosEntity)
    {
        ListingPhotoId = listingPhotosEntity.ListingPhotoId;
        BookListingId = listingPhotosEntity.BookListingId;
    }

    [JsonConstructor]
    public ListingPhotosGetDto(int listingPhotoId, int bookListingId)
    {
        ListingPhotoId = listingPhotoId;
        BookListingId = bookListingId;
    }
}
