using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `ListingPhotoPostDto`.</summary>
public class ListingPhotoDtoOutput
{
    public required ListingPhotoEntity ListingPhotoEntity { get; set; }
}

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class ListingPhotoPostDto : IPostDto<ListingPhotoDtoOutput>
{
    [JsonPropertyName("listingPhotoId")]
    public int ListingPhotoId { get; set; }

    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    public ListingPhotoDtoOutput Map()
    {
        var listingPhotoEntity = new ListingPhotoEntity
        {
            ListingPhotoId = 0,
            BookListingId = 0,
        };

        return new ListingPhotoDtoOutput
        {
            ListingPhotoEntity = listingPhotoEntity
        };
    }
}
