using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Users.UserId;

public class PatchTests
{
    [Fact]
    public async Task SimpleTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "2" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "PATCH";

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

        context.Logger.LogDebug($"PATCH /api/users/{{userId}} SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<UserGetDto>(response.Body);
        Assert.NotNull(entities);
        Assert.Equal("John", entities.FirstName);
        Assert.Equal("Doe", entities.LastName);
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
