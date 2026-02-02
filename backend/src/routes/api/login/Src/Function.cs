using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSec.Cryptography;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Login;

public class Function
{
    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        string method;
        try
        {
            method = request.RequestContext.Http.Method;
        }
        catch (NullReferenceException)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request is not a valid AWS API Gateway HTTP API V2 request.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
        }

        switch (method)
        {
            case "POST":
                return await post(request, context);
            default:
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 405,
                    Body = $"{{\"error\":\"Method '{method}' not allowed.\"}}",
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                        { "Allow", "POST" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        using var oteContext = new OteContext();

        var deserializeResult = ApiFunctions.DeserializeJsonEntity<SessionTokenPostDto>(request, context.Logger);
        if (!deserializeResult.Ok)
            return deserializeResult.UnwrapError();

        var sessionTokenPostDto = deserializeResult.Unwrap();

        var user = await oteContext
            .Users
            .Where(e => e.DeletedAt == null)
            .Where(e => e.Username == sessionTokenPostDto.Username)
            .FirstOrDefaultAsync();

        if (user == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"User not found.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };

        var sessionToken = new SessionTokenCacheEntity();
        sessionToken.User = user;
        sessionToken.UserId = user.UserId;

        var password = await oteContext
            .Argon2idPasswords
            .Where(e => e.UserId == user.UserId)
            .FirstOrDefaultAsync();

        if (password == null)
        {
            context.Logger.LogError("POST /api/login: User somehow has no password");
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 500,
                Body = $"{{\"error\":\"Internal server error.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
        }

        byte[] bytePassword = new byte[Encoding.UTF8.GetByteCount(sessionTokenPostDto.Password)];
        Encoding.UTF8.GetBytes(sessionTokenPostDto.Password, bytePassword);

        var parameters = new Argon2Parameters();
        parameters.DegreeOfParallelism = password.Parallelism;
        parameters.MemorySize = password.MemoryCost;
        parameters.NumberOfPasses = password.Iterations;

        var argon = PasswordBasedKeyDerivationAlgorithm.Argon2id(parameters);
        var hash = argon.DeriveBytes(bytePassword, password.Salt, 16);

        if (!CryptographicOperations.FixedTimeEquals(hash, password.Hash))
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"User not found.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };

        var insertedSessionTokenEntry = await oteContext
            .SessionTokens
            .AddAsync(sessionToken);

        try
        {
            await oteContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                {
                    var n = (NpgsqlException)i;
                    return ApiFunctions.HandleRepoError(n, context.Logger);
                }
            }
            throw;
        }

        var insertedSessionToken = insertedSessionTokenEntry.Entity;
        var sessionTokenGetDto = new SessionTokenGetDto(insertedSessionToken);
        var sessionTokenGetDtoJson = JsonSerializer.Serialize(sessionTokenGetDto);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = sessionTokenGetDtoJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
