using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using System.Text.Json;
using OTE.Data.EFCore.Factories;
using OTE.Data.EFCore.Repositories;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes;

public class Function
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Any, "/")]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var factory = new OteContextFactory();
        var dbContext = factory.CreateDbContext();
        var schoolRepo = new SchoolRepo(dbContext, context.Logger);

        var schools = await schoolRepo.GetAll();
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
