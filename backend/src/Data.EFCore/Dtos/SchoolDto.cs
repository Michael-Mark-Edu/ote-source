using System.ComponentModel.DataAnnotations;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IDto` corresponding to `SchoolEntity`.</summary>
public class SchoolDto : IDto<SchoolEntity>
{
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Acronym { get; set; } = string.Empty;

    [MaxLength(2)]
    [MinLength(2)]
    public string? State { get; set; }

    [MaxLength(255)]
    public string? City { get; set; }

    public SchoolEntity? Map(IEnumerable<IEnumerable<object>>? references)
    {
        return new SchoolEntity
        {
            SchoolId = 0,
            Name = Name,
            Acronym = Acronym,
            State = State,
            City = City
        };
    }
}
