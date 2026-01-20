using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IInsertableDto` corresponding to `SchoolEntity`.</summary>
public class SchoolDto : IInsertableDto<SchoolEntity>
{
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

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    public SchoolEntity Map()
    {
        return new SchoolEntity
        {
            SchoolId = 0,
            Name = Name,
            Acronym = Acronym,
            State = State,
            City = City,
            CreatedAt = CreatedAt,
            DeletedAt = DeletedAt
        };
    }
}
