using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `CoverPhotoPostDto`.</summary>
public class CoverPhotoPostDtoOutput
{
    public required CoverPhotoEntity CoverPhotoEntity { get; set; }
}

/// <summary>`IPostDto` for inserting from a POST request.</summary>
public class CoverPhotoPostDto : IPostDto<CoverPhotoPostDtoOutput>
{
    [JsonPropertyName("coverPhotoId")]
    public int CoursPhotoId { get; set; }

    [JsonPropertyName("bookId")]
    public int BookId { get; set; }


    public CoverPhotoPostDtoOutput Map()
    {
        var coverPhotoEntity = new CoverPhotoEntity
        {
            CoverPhotoId = 0,
            BookId = 0
        };

        return new CoverPhotoPostDtoOutput
        {
            CoverPhotoEntity = coverPhotoEntity
        };
    }
}
