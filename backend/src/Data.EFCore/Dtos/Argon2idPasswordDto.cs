using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Dtos;

/// <summary>`IDto` corresponding to `Argon2idPasswordEntity`.</summary>
public class Argon2idPasswordDto : IDto<Argon2idPasswordEntity>
{
    public byte Version { get; set; }

    public int MemoryCost { get; set; }

    public int Iterations { get; set; }

    public byte Parallelism { get; set; }

    public byte[] Salt { get; set; } = null!;

    public byte[] Hash { get; set; } = null!;

    public Argon2idPasswordEntity Map()
    {
        return new Argon2idPasswordEntity
        {
            Argon2idPasswordId = 0,
            Version = Version,
            MemoryCost = MemoryCost,
            Iterations = Iterations,
            Parallelism = Parallelism,
            Salt = Salt,
            Hash = Hash
        };
    }
}
