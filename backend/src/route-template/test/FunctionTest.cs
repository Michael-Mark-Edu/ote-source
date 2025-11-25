using Xunit;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;

namespace routes.Tests;

public class FunctionTest
{
    [Fact]
    public void TestFunction()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayProxyRequest();
        var response = function.FunctionHandler(request, context);

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("<html><body><h1>Hello, world!</h1></body></html>", response.Body);
    }
}
