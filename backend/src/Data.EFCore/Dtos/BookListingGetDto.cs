using OTE.Data.EFCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for returning book listings for a GET request.</summary>
public class BookListingGetDto : IGetDto<BookListingEntity>
{
    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    [JsonPropertyName("condition")]
    public string Condition { get; set; } = string.Empty;

    // Purchase Type (Rent, Trade, Sell)
    [JsonPropertyName("purchaseType")]
    public string PurchaseType { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public string? Price { get; set; } = null;

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("isbn")]
    public string ISBN { get; set; }

    public BookListingGetDto(BookListingEntity bookListingEntity)
    {
        BookListingId = bookListingEntity.BookListingId;
        Condition = bookListingEntity.Condition;
        PurchaseType = bookListingEntity.PurchaseType;
        Price = bookListingEntity.Price;
        UserId = bookListingEntity.UserId;
        BookISBN = bookListingEntity.BookISBN;
    }

    [JsonConstructor]
    public BookListingGetDto(int bookListingId, string condition, string purchaseType, string? price, int userId, string isbn)
    {
        BookListingId = bookListingId;
        Condition = condition;
        PurchaseType = purchaseType;
        Price = price;
        UserId = userId;
        ISBN = isbn;
    }
}
