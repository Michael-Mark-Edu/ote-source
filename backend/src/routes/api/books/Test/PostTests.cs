using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Books;

public class PostTests
{
    [Fact]
    public async Task SimpleTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";

        request.Body =
        """
        {
            "isbn": "20000",
            "title": "__TEST_TITLE",
            "authors": "__TEST_AUTHORS",
            "publishers": "__TEST_PUBLISHERS",
            "description": "__TEST_DESCRIPTION",
            "publishDate": "1995-01-01T01:00:00Z"
        }
        """;
        context.Logger.LogDebug($"POST /api/books SimpleTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/books SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<BookGetDto>(response.Body);
        Assert.NotNull(entities);
    }

    [Fact]
    public async Task MissingFieldTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";

        request.Body =
        """
        {
            "isbn": "20001",
            "title": "__TEST_TITLE",
            "authors": "__TEST_AUTHORS",
            "description": "__TEST_DESCRIPTION",
            "publishDate": "1995-01-01T01:00:00Z"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/books MissingFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async Task DuplicateTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";

        request.Body =
        """
        {
            "isbn": "20002",
            "title": "__TEST_TITLE",
            "authors": "__TEST_AUTHORS",
            "publishers": "__TEST_PUBLISHERS",
            "description": "__TEST_DESCRIPTION",
            "publishDate": "1995-01-01T01:00:00Z"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/books DuplicateTest | response.Body 1 = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        response = await function.FunctionHandler(request, context);

        context.Logger.LogDebug($"POST /api/books DuplicateTest | response.Body 2 = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async Task ExtraFieldTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";

        request.Body =
        """
        {
            "isbn": "20003",
            "title": "__TEST_TITLE",
            "authors": "__TEST_AUTHORS",
            "publishers": "__TEST_PUBLISHERS",
            "description": "__TEST_DESCRIPTION",
            "publishDate": "1995-01-01T01:00:00Z",
            "cost": "$19.99"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/books MissingFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async Task DuplicateFieldTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";

        request.Body =
        """
        {
            "isbn": "20004",
            "title": "__TEST_TITLE",
            "authors": "__TEST_AUTHORS",
            "authors": "__TEST_AUTHORS",
            "publishers": "__TEST_PUBLISHERS",
            "description": "__TEST_DESCRIPTION",
            "publishDate": "1995-01-01T01:00:00Z"
        }
        """;
        context.Logger.LogDebug($"POST /api/books DuplicateFieldTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/books DuplicateFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }
}
