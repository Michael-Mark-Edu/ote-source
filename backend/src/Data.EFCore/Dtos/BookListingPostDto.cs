using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `BookListingPostDto`.</summary>
public class BookListingPostDtoOutput
{
    public required BookListingEntity BookListingEntity { get; set; }
}

/// <summary>`IPostDto` for inserting user/password pairs from a POST request.</summary>
public class BookListingPostDto : IPostDto<BookListingPostDtoOutput>
{
    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    [MaxLength(20)]
    [JsonPropertyName("condition")]
    public string Condition { get; set; } = string.Empty;

    [MaxLength(100)]
    [JsonPropertyName("purchaseType")]
    public string PurchaseType { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("price")]
    public string? Price { get; set; } = null;

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("isbn")]
    public int ISBN { get; set; }

    public BookListingPostDtoOutput Map()
    {
        var bookListingEntity = new BookListingEntity
        {
            BookListingId = BookListingId,
            Condition = Condition,
            PurchaseType = PurchaseType,
            Price = Price,
            UserId = UserId,
            ISBN = ISBN
        };

        return new BookListingPostDtoOutput
        {
            BookListingEntity = bookListingEntity
        };
    }
}
