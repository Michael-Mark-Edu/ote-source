using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Users.UserId;

public class PatchTests
{
    [Fact]
    public async Task UserSelfPatchTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "2" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "John",
            "lastName": "Doe"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} UserSelfPatchTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<UserGetDto>(response.Body);
        Assert.NotNull(entities);
        Assert.Equal("John", entities.FirstName);
        Assert.Equal("Doe", entities.LastName);
    }

    [Fact]
    public async Task SelfRouteTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "self" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "Johhn",
            "lastName": "Doee"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} SelfRouteTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<UserGetDto>(response.Body);
        Assert.NotNull(entities);
        Assert.Equal("Johhn", entities.FirstName);
        Assert.Equal("Doee", entities.LastName);
    }

    [Fact]
    public async Task AdminPatchTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "Dohn",
            "lastName": "Joe"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} AdminPatchTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<UserGetDto>(response.Body);
        Assert.NotNull(entities);
        Assert.Equal("Dohn", entities.FirstName);
        Assert.Equal("Joe", entities.LastName);
    }

    [Fact]
    public async Task IllegalCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=0", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "Dohn",
            "lastName": "Joe"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} IllegalCookiesTest | response.Body = {response.Body}");
        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task WrongCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "Dohn",
            "lastName": "Joe"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} WrongCookiesTest | response.Body = {response.Body}");
        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task NoCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "Dohn",
            "lastName": "Joe"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} NoCookiesTest | response.Body = {response.Body}");
        Assert.Equal(401, response.StatusCode);
    }

    [Fact]
    public async Task DuplicateFieldTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "2" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        """
        {
            "firstName": "John",
            "firstName": "John"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} DuplicateFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }

    [Fact]
    public async Task FakeFieldTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "2" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        request.IsBase64Encoded = false;
        request.Body =
        $$"""
        {
            "createdAt": "{{DateTime.UtcNow}}"
        }
        """;

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} FakeFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }
}
