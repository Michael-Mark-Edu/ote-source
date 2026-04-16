using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>Output type for `SchoolPostDto`.</summary>
public class SchoolPostDtoOutput
{
    public required SchoolEntity SchoolEntity { get; set; }
}

/// <summary>`IPostDto` for inserting user/password pairs from a POST request.</summary>
public class SchoolPostDto : IPostDto<SchoolPostDtoOutput>
{
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

    public SchoolPostDtoOutput Map()
    {
        var schoolEntity = new SchoolEntity
        {
            SchoolId = 0,
            Name = Name,
            Acronym = Acronym,
            State = State,
            City = City

        };

        return new SchoolPostDtoOutput
        {
            SchoolEntity = schoolEntity
        };
    }
}
