using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using System.Text.Json;
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
    [RestApi(LambdaHttpMethod.Any, "/api/users")]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();
        _userRepo = new UserRepo(_dbContext, context.Logger);

        switch (request.HttpMethod)
        {
            case "GET":
                return await get(request, context);
            case "POST":
                return await post(request, context);
            default:
                return new APIGatewayHttpApiV2ProxyResponse {
                    StatusCode = 405,
                    Body = "Invalid HTTP method " + request.HttpMethod,
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "text/plain" },
                        { "Allow", "GET, POST" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var entities = await _userRepo.GetAll();
        if (entities == null)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 500,
                Body = "Could not read from database",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var entitiesJson = JsonSerializer.Serialize(entities);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = entitiesJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayProxyRequest request, ILambdaContext context)
    {
        UserDto? dto;
        try
        {
            dto = JsonSerializer.Deserialize<UserDto>(request.Body);
        }
        catch (JsonException e)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = $"Request body contains invalid JSON data.\n\n{e.Message}",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
        catch (Exception e)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 500,
                Body = $"Unknown exception occured.\n\n{e.Message}",
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

        var inserted = await _userRepo.Insert(dto.Map());
        if (inserted == null)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 500,
                Body = "Could not access data from database.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var insertedJson = JsonSerializer.Serialize(inserted.Entity);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = insertedJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
