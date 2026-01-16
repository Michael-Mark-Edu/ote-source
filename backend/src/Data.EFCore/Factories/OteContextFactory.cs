using Amazon.RDS.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OTE.Data.EFCore.Contexts;

namespace OTE.Data.EFCore.Factories;

/// <summary>Factory-pattern class for getting `OteContext`s.</summary>
public class OteContextFactory : IDesignTimeDbContextFactory<OteContext>
{
    /// <summary>Creates an `OteContext` object. Identical to CreateDbContext([]).</summary>
    /// <returns>A configured `OteContext` instance.</returns>
    public OteContext CreateDbContext()
    {
        return CreateDbContext([]);
    }

    /// <summary>Interface method for `IDesignTimeDbContextFactory`.</summary>
    /// <param name="args">Unused</param>
    /// <returns>A configured `OteContext` instance.</returns>
    /// <remarks>
    /// This method is only implemented for design-time initialization.
    /// Use the parameterless variant instead.
    /// </remarks>
    public OteContext CreateDbContext(string[] args)
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

        var optionsBuilder = new DbContextOptionsBuilder<OteContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new OteContext(optionsBuilder.Options);
    }
}
