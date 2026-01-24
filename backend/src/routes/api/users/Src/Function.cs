using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text.Json;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Factories;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Users;

public class Function
{
    private OteContextFactory _factory = null!;
    private OteContext _dbContext = null!;

    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();

        string method = request.RequestContext.Http.Method;

        switch (method)
        {
            case "GET":
                return await get(request, context);
            case "POST":
                return await post(request, context);
            default:
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 405,
                    Body = $"{{\"error\":\"Method '{method}' not allowed.\"}}",
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                        { "Allow", "GET, POST" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var users = await _dbContext
            .Users
            .Where(e => e.DeletedAt == null)
            .ToListAsync();

        var userGetDtos = users.Select((e, i) => new UserGetDto(e));
        var usersJson = JsonSerializer.Serialize(userGetDtos);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = usersJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var deserializeResult = ApiFunctions.DeserializeJson<UserPostDto>(request, context.Logger);
        if (!deserializeResult.Ok)
            return deserializeResult.UnwrapError();

        var userPostDtoOutput = deserializeResult.Unwrap().Map();

        var insertPasswordAsync = _dbContext
            .Argon2idPasswords
            .AddAsync(userPostDtoOutput.Argon2idPasswordEntity);

        var insertUserAsync = _dbContext
            .Users
            .AddAsync(userPostDtoOutput.UserEntity);

        await insertPasswordAsync;
        var insertedUserEntry = await insertUserAsync;

        try
        {
            await _dbContext.SaveChangesAsync();
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

        var insertedUser = insertedUserEntry.Entity;
        var userGetDto = new UserGetDto(insertedUser);
        var userGetDtoJson = JsonSerializer.Serialize(userGetDto);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = userGetDtoJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
