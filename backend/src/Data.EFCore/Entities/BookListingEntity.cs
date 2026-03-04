using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing book listings.</summary>
public class BookListingEntity
{
    [Key]
    [JsonPropertyName("bookListingId")]
    public int BookListingId { get; set; }

    [MaxLength(20)]
    [JsonPropertyName("condition")]
    public string Condition { get; set; } = string.Empty;

    // Purchase Type (Rent, Trade, Sell)
    [MaxLength(100)]
    [JsonPropertyName("purchaseType")]
    public string PurchaseType { get; set; } = string.Empty;

    [MaxLength(255)]
    [JsonPropertyName("price")]
    public string? Price { get; set; } = null;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [JsonIgnore]
    [ForeignKey("UserId")]
    public UserEntity Seller { get; set; } = null!;

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    // NAV Prop
    [JsonIgnore]
    public BookEntity Book { get; set; } = null!;

    // FK
    [JsonPropertyName("isbn")]
    public int ISBN { get; set; }
}
