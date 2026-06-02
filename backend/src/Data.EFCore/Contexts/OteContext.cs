using Amazon.RDS.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Contexts;

/// <summary>`DbContext` representing the OTE database schema.</summary>
public class OteContext : DbContext
{
    public DbSet<Argon2idPasswordEntity> Argon2idPasswords { get; set; } = null!;
    public DbSet<BookEntity> Books { get; set; } = null!;
    public DbSet<BookListingEntity> BookListings { get; set; } = null!;
    public DbSet<ListingPhotoEntity> ListingPhotos { get; set; } = null!;
    public DbSet<SchoolEntity> Schools { get; set; } = null!;
    public DbSet<SessionTokenCacheEntity> SessionTokens { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("public");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? lambdaMode = Environment.GetEnvironmentVariable("OTE_LAMBDA");
        string connectionString;

        if (lambdaMode == "1")
        {
            var password = RDSAuthTokenGenerator.GenerateAuthToken(
                "ote-db.cvywoma8glcn.us-west-2.rds.amazonaws.com",
                5432,
                "api_user");

            connectionString = $"Host=ote-db.cvywoma8glcn.us-west-2.rds.amazonaws.com; Database=otedb; Username=api_user; Password={password}; SSL Mode=Require; Trust Server Certificate=true;";
        }
        else
        {
            string secretsUuid = "7dd46374-7b55-442e-b4b3-1ae375510d4e";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(secretsUuid)
                .Build();

            connectionString = configuration.GetConnectionString("OteDb")
                ?? throw new Exception("Could not get connections string");
        }

        optionsBuilder.UseNpgsql(connectionString);
    }
}
