using System.ComponentModel.DataAnnotations;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing schools.</summary>
public class SchoolEntity
{
    [Key]
    public int SchoolId { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Acronym { get; set; } = string.Empty;

    [MaxLength(2)]
    [MinLength(2)]
    public string? State { get; set; } = null;

    [MaxLength(255)]
    public string? City { get; set; } = null;
}
