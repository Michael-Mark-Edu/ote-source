using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace routes;

public class Function
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Any, "/")]
    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = "<html><body><h1>Hello, world!</h1></body></html>",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/html" } }
        };
    }
}
