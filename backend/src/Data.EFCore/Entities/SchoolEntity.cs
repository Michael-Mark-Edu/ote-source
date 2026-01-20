using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing schools.</summary>
public class SchoolEntity
{
    [Key]
    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    [MaxLength(255)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(10)]
    [JsonPropertyName("acronym")]
    public string Acronym { get; set; } = string.Empty;

    [MaxLength(2)]
    [MinLength(2)]
    [JsonPropertyName("state")]
    public string? State { get; set; } = null;

    [MaxLength(255)]
    [JsonPropertyName("city")]
    public string? City { get; set; } = null;
}
