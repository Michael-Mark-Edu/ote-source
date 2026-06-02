using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace OTE.Routes.Api.Books.BookISBN;

public class GeneralHttpTests
{
    [Fact]
    public async Task LowercaseGet405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "bookISBN", "12347" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "get";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, PATCH, DELETE", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/books/{{bookISBN}} LowercaseGet405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task GetGet405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "bookISBN", "12347" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GETGET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, PATCH, DELETE", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/books/{{bookISBN}} GetGet405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task Put405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "bookISBN", "12347" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PUT";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, PATCH, DELETE", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/books/{{bookISBN}} Delete405Test | response.Body = {response.Body}");
    }

    [Fact]
    public async Task Blank405Test()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "bookISBN", "12347" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(405, response.StatusCode);
        Assert.Equal("GET, PATCH, DELETE", response.Headers["Allow"]);

        context.Logger.LogDebug($"GENERAL /api/books/{{bookISBN}} Blank405Test | response.Body = {response.Body}");
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

        context.Logger.LogDebug($"GENERAL /api/books/{{bookISBN}} UninitializedRequestTest | response.Body = {response.Body}");
    }
}
