using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for cover photo in a GET request.</summary>
public class CoverPhotoGetDto : IGetDto<CoverPhotoEntity>
{
    [JsonPropertyName("coverPhotoId")]
    public int CoverPhotoId { get; set; }

    [JsonPropertyName("bookId")]
    public int BookId { get; set; }


    public CoverPhotoGetDto(CoverPhotoEntity coverPhotoEntity)
    {
        CoverPhotoId= coverPhotoEntity.CoverPhotoId;
        BookId = coverPhotoEntity.BookId;
    }

    [JsonConstructor]
    public CoverPhotoGetDto(int coverPhotoId, int bookId)
    {
        CoverPhotoId = coverPhotoId;
        BookId = bookId;
        
    }
}
