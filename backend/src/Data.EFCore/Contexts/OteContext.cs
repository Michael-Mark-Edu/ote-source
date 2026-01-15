using Microsoft.EntityFrameworkCore;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Contexts;

/// <summary>`DbContext` representing the OTE database schema.</summary>
/// <param name="options">`DbContextOptions` object passed to the `DbContext` constructor.</param>
public class OteContext(DbContextOptions<OteContext> options) : DbContext(options)
{
    public DbSet<Argon2idPasswordEntity> Argon2idPasswords { get; set; } = null!;
    public DbSet<SchoolEntity> Schools { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("public");
    }
}
