using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using System.Text.Json;
using OTE.Common;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Factories;
using OTE.Data.EFCore.Repositories;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Users;

public class Function
{
    private OteContextFactory _factory = null!;
    private OteContext _dbContext = null!;
    private UserRepo _userRepo = null!;
    private Argon2idPasswordRepo _passwordRepo = null!;

    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();
        _userRepo = new UserRepo(_dbContext);
        _passwordRepo = new Argon2idPasswordRepo(_dbContext);

        string method = request.RequestContext.Http.Method;

        switch (method)
        {
        case "GET":
            return await get(request, context);
        case "POST":
            return await post(request, context);
        default:
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 405,
                Body = $"Method \"{method}\" Not Allowed",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                    { "Allow", "GET, POST" }
                }
            };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var entitiesResult = await _userRepo.GetAll();
        if (!entitiesResult.Ok)
        {
            var error = entitiesResult.UnwrapError();
            var errorData = DatabaseErrorHandler.Parse(error);

            if (errorData.LogMessage != null)
                context.Logger.LogError($"UserRepo.GetAll() error: {errorData.LogMessage}");

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = errorData.HttpStatus,
                Body = errorData.BodyMessage,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var entities = entitiesResult.Unwrap();
        var entityGetDtos = entities.Select((e, i) => new UserGetDto(e));
        var entitiesJson = JsonSerializer.Serialize(entityGetDtos);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = entitiesJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        UserPostDto? dto;
        try
        {
            string body;
            if (request.IsBase64Encoded)
                body = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.Body));
            else
                body = request.Body;

            dto = JsonSerializer.Deserialize<UserPostDto>(body);
        }
        catch (JsonException e)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = $"Request body contains invalid JSON data. ${e.Message}",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
        catch (ArgumentNullException)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = $"Request body must contain JSON data of the entity to insert.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
        catch (Exception e)
        {
            context.Logger.LogError($"Unknown exception occured: {e.Message}");
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 500,
                Body = "Internal Server Error",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
        if (dto == null)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = "Failed to deserialize request body.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var dtoResults = dto.Map();

        var passwordInsertResult = await _passwordRepo.Insert(dtoResults.Argon2idPasswordEntity);
        if (!passwordInsertResult.Ok)
        {
            var error = passwordInsertResult.UnwrapError();
            var errorData = DatabaseErrorHandler.Parse(error);

            if (errorData.LogMessage != null)
                context.Logger.LogError($"Argon2idPasswordRepo.Insert() error: {errorData.LogMessage}");

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = errorData.HttpStatus,
                Body = errorData.BodyMessage,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var userInsertResult = await _userRepo.Insert(dtoResults.UserEntity);
        if (!userInsertResult.Ok)
        {
            var error = userInsertResult.UnwrapError();
            var errorData = DatabaseErrorHandler.Parse(error);

            if (errorData.LogMessage != null)
                context.Logger.LogError($"UserRepo.Insert() error: {errorData.LogMessage}");

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = errorData.HttpStatus,
                Body = errorData.BodyMessage,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        // TODO: Add IGetDto

        var inserted = userInsertResult.Unwrap();
        var insertedGetDto = new UserGetDto(inserted.Entity);
        var insertedJson = JsonSerializer.Serialize(insertedGetDto);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = insertedJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
