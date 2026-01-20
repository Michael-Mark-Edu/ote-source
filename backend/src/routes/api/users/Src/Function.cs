using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
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

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Any, "/api/users")]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();
        _userRepo = new UserRepo(_dbContext, context.Logger);

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
        var entitiesJson = JsonSerializer.Serialize(entities);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = entitiesJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        UserDto? dto;
        try
        {
            string body;
            if (request.IsBase64Encoded)
                body = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.Body));
            else
                body = request.Body;

            dto = JsonSerializer.Deserialize<UserDto>(body);
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

        dto.CreatedAt = dto.CreatedAt.ToUniversalTime();
        var insertResult = await _userRepo.Insert(dto.Map());

        if (!insertResult.Ok)
        {
            var error = insertResult.UnwrapError();
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

        var inserted = insertResult.Unwrap();
        var insertedJson = JsonSerializer.Serialize(inserted.Entity);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = insertedJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
