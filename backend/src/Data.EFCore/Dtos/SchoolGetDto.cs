using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IGetDto` for returning schools for a GET request.</summary>
public class SchoolGetDto : IGetDto<SchoolEntity>
{
    [JsonPropertyName("schoolId")]
    public int SchoolId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("acronym")]
    public string Acronym { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string? State { get; set; } = null;

    [JsonPropertyName("city")]
    public string? City { get; set; } = null;

    public SchoolGetDto(SchoolEntity schoolEntity)
    {
        SchoolId = schoolEntity.SchoolId;
        Name = schoolEntity.Name;
        Acronym = schoolEntity.Acronym;
        State = schoolEntity.State;
        City = schoolEntity.City;
    }

    [JsonConstructor]
    public SchoolGetDto(int schoolId, string name, string acronym, string? state, string? city)
    {
        SchoolId = schoolId;
        Name = name;
        Acronym = acronym;
        State = state;
        City = city;
    }
}
