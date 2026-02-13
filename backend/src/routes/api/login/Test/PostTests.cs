using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Login;

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
            "username": "johndoe",
            "password": "password1"
        }
        """;
        context.Logger.LogDebug($"POST /api/login SimpleTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/login SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<SessionTokenGetDto>(response.Body);
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
            "password": "password1"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/login MissingFieldTest | response.Body = {response.Body}");
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
            "username": "charliedavis",
            "password": "password5"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/login DuplicateTest | response.Body 1 = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var response2 = await function.FunctionHandler(request, context);

        context.Logger.LogDebug($"POST /api/login DuplicateTest | response.Body 2 = {response2.Body}");
        Assert.Equal(200, response2.StatusCode);

        Assert.NotEqual(response.Body, response2.Body);
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
        $$"""
        {
            "username": "johndoe",
            "password": "password1"
            "createdAt": "{{DateTime.UtcNow}}",
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/login MissingFieldTest | response.Body = {response.Body}");
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
            "username": "johndoe",
            "password": "password1",
            "password": "password1"
        }
        """;
        context.Logger.LogDebug($"POST /api/login DuplicateFieldTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/login DuplicateFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async Task WrongPasswordTest()
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
            "username": "johndoe",
            "password": "password2"
        }
        """;
        context.Logger.LogDebug($"POST /api/login SimpleTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/login SimpleTest | response.Body = {response.Body}");
        Assert.Equal(404, response.StatusCode);
    }
}
