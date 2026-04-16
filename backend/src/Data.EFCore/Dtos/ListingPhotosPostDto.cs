using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `ListingPhotosPostDto`.</summary>
public class ListingPhotosDtoOutput
{
    public required ListingPhotosEntity ListingPhotosEntity { get; set; }
}

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class ListingPhotosPostDto : IPostDto<ListingPhotosDtoOutput>
{
    [JsonPropertyName("listingPhotosId")]
    public int ListingPhotosId { get; set; }

    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    public ListingPhotosDtoOutput Map()
    {
        var listingPhotosEntity = new ListingPhotosEntity
        {
            ListingPhotoId = 0,
            BookListingId = 0,
        };

        return new ListingPhotosDtoOutput
        {
            ListingPhotosEntity = listingPhotosEntity
        };
    }
}
