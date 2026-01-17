using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using System.Text.Json;
using OTE.Data.EFCore.Contexts;
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

        if (request.HttpMethod == "GET")
        {
            return await get(request, context);
        }
        else
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 405,
                Body = "Invalid HTTP method " + request.HttpMethod,
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                    { "Allow", "GET" }
                }
            };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var schools = await _userRepo.GetAll();
        if (schools == null)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 500,
                Body = "Could not read from database",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var schoolsJson = JsonSerializer.Serialize(schools);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = schoolsJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
