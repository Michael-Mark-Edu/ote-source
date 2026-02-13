using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Npgsql;

namespace OTE.Common.Api;

public static partial class ApiFunctions
{
    /// <summary>Gets an HTTP response from an `NpgsqlException` while also handling logging.</summary>
    /// <param name="error">The `NpgsqlException` to parse.</param>
    /// <param name="logger">The `ILambdaLogger` instance used for logging.</param>
    /// <returns>An `APIGatewayHttpApiV2ProxyResponse` ready to be sent to the user.</returns>
    public static APIGatewayHttpApiV2ProxyResponse HandleRepoError(NpgsqlException error, ILambdaLogger logger)
    {
        var errorData = DatabaseErrorHandler.Parse(error);

        if (errorData.LogMessage != null)
            logger.LogError($"Argon2idPasswordRepo.Insert() error: {errorData.LogMessage}");

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = errorData.HttpStatus,
            Body = $"{{\"error\":\"{errorData.BodyMessage}\"}}",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
