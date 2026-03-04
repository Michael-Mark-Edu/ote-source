using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace OTE.Routes.Api.Listings;

public class GeneralHttpTests
{
    [Fact]
    public async Task LowercaseGet405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "get";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, POST", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/listings LowercaseGet405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task GetGet405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GETGET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, POST", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/listings GetGet405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task Delete405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, POST", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/listings Delete405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task Blank405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, POST", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/listings Blank405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task UninitializedRequestTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(400, response.StatusCode);

        context.Logger.LogDebug($"GENERAL /api/listings UninitializedRequestTest | response.Body = {response.Body}");
    }
}
