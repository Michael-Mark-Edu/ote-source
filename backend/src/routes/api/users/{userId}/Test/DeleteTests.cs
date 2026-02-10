using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace OTE.Routes.Api.Users.UserId;

public class DeleteTests
{
    [Fact]
    public async Task SimpleTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "3" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/users/{{userId}} SimpleTest | response.Body = {response.Body}");
        Assert.Equal(204, response.StatusCode);

        request.RequestContext.Http.Method = "GET";

        response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task NegativeIdTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "-1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/users/{{userId}} NegativeIdTest | response.Body = {response.Body}");
        Assert.Equal(404, response.StatusCode);
    }
}
