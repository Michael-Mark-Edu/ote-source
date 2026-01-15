using Amazon.RDS.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OTE.Data.EFCore.Contexts;

namespace OTE.Data.EFCore.Factories;

/// <summary>Factory-pattern class for getting `OteContext`s.</summary>
public class OteContextFactory : IDesignTimeDbContextFactory<OteContext>
{
    /// <summary>Interface method for `IDesignTimeDbContextFactory`.</summary>
    /// <param name="args"></param>
    /// <returns>A configured `OteContext` instance.</returns>
    public OteContext CreateDbContext(string[] args)
    {
        string connectionString;

        if (args.Count() >= 1 && args[0] == "lambda")
        {
            var password = RDSAuthTokenGenerator.GenerateAuthToken(
                "ote-db.cvywoma8glcn.us-west-2.rds.amazonaws.com",
                5432,
                "api_user");

            connectionString = $"Host=ote-db.cvywoma8glcn.us-west-2.rds.amazonaws.com; Database=otedb; Username=api_user; Password={password}; SSL Mode=Require; Trust Server Certificate=true;";
        }
        else if (args.Count() >= 1 && args[0] == "test")
        {
            string secretsUuid = "7dd46374-7b55-442e-b4b3-1ae375510d4e";
            if (args.Count() >= 2)
                secretsUuid = args[1];
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(secretsUuid)
                .Build();

            connectionString = configuration.GetConnectionString("OteDb")
                ?? throw new Exception("Could not get connections string");
        }
        else
        {
            throw new Exception("No args specified");
        }

        var optionsBuilder = new DbContextOptionsBuilder<OteContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new OteContext(optionsBuilder.Options);
    }
}
