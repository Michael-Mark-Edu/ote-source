using System.ComponentModel.DataAnnotations;

namespace OTE.Data.EFCore.Entities;

/// <summary>Entity type for representing password hashes.</summary>
public class Argon2idPasswordEntity
{
    [Key]
    public int Argon2idPasswordId { get; set; }

    public byte Version { get; set; }

    public int MemoryCost { get; set; }

    public int Iterations { get; set; }

    public byte Parallelism { get; set; }

    public byte[] Salt { get; set; } = null!;

    public byte[] Hash { get; set; } = null!;
}
