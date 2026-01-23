using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Npgsql;
using System.Text.Json;

namespace OTE.Common.Api;

public static class ApiFunctions
{
    public static APIGatewayHttpApiV2ProxyResponse HandleRepoError(NpgsqlException error, ILambdaLogger logger)
    {
        var errorData = DatabaseErrorHandler.Parse(error);

        if (errorData.LogMessage != null)
            logger.LogError($"Argon2idPasswordRepo.Insert() error: {errorData.LogMessage}");

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = errorData.HttpStatus,
            Body = $"{{\"error\":{errorData.BodyMessage}}}",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    public static Result<TTarget, APIGatewayHttpApiV2ProxyResponse> DeserializeJson<TTarget>(APIGatewayHttpApiV2ProxyRequest request, ILambdaLogger logger)
    {
        TTarget? target;
        try
        {
            string body;
            if (request.IsBase64Encoded)
                body = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.Body));
            else
                body = request.Body;

            target = JsonSerializer.Deserialize<TTarget>(body);
        }
        catch (JsonException e)
        {
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body contains invalid JSON data. ${e.Message}\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (ArgumentNullException)
        {
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body must contain JSON data of the entity to insert.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (Exception e)
        {
            logger.LogError($"Unknown exception occured: {e.Message}");
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 500,
                Body = $"{{\"error\":\"Internal server error.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        if (target == null)
        {
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Failed to deserialize request body.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }

        return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewOk(target);
    }
}
