using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Users;

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
            "username": "API_USERS_POST_SimpleTest_USERNAME",
            "emailAddress": "API_USERS_POST_SimpleTest_EMAIL",
            "firstName": "API_USERS_POST_SimpleTest_FIRSTNAME",
            "lastName": "API_USERS_POST_SimpleTest_LASTNAME",
            "middleName": "API_USERS_POST_SimpleTest_MIDDLENAME",
            "password": "API_USERS_POST_SimpleTest_PASSWORD",
            "schoolId": 1
        }
        """;
        context.Logger.LogDebug($"POST /api/users SimpleTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/users SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<UserGetDto>(response.Body);
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
            "emailAddress": "API_USERS_POST_MissingFieldTest_EMAIL",
            "firstName": "API_USERS_POST_MissingFieldTest_FIRSTNAME",
            "lastName": "API_USERS_POST_MissingFieldTest_LASTNAME",
            "middleName": "API_USERS_POST_MissingFieldTest_MIDDLENAME",
            "password": "API_USERS_POST_MissingFieldTest_PASSWORD",
            "schoolId": 1
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/users MissingFieldTest | response.Body = {response.Body}");
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
            "username": "API_USERS_POST_DuplicateTest_USERNAME",
            "emailAddress": "API_USERS_POST_DuplicateTest_EMAIL",
            "firstName": "API_USERS_POST_DuplicateTest_FIRSTNAME",
            "lastName": "API_USERS_POST_DuplicateTest_LASTNAME",
            "middleName": "API_USERS_POST_DuplicateTest_MIDDLENAME",
            "password": "API_USERS_POST_DuplicateTest_PASSWORD",
            "schoolId": 1
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/users DuplicateTest | response.Body 1 = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        response = await function.FunctionHandler(request, context);

        context.Logger.LogDebug($"POST /api/users DuplicateTest | response.Body 2 = {response.Body}");
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
        $$"""
        {
            "username": "API_USERS_POST_ExtraFieldTest_USERNAME",
            "emailAddress": "API_USERS_POST_ExtraFieldTest_EMAIL",
            "firstName": "API_USERS_POST_ExtraFieldTest_FIRSTNAME",
            "lastName": "API_USERS_POST_ExtraFieldTest_LASTNAME",
            "middleName": "API_USERS_POST_ExtraFieldTest_MIDDLENAME",
            "password": "API_USERS_POST_ExtraFieldTest_PASSWORD",
            "createdAt": "{{DateTime.UtcNow}}",
            "schoolId": 1
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/users MissingFieldTest | response.Body = {response.Body}");
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
            "username": "API_USERS_POST_DuplicateFieldTest_USERNAME",
            "emailAddress": "API_USERS_POST_DuplicateFieldTest_EMAIL",
            "firstName": "API_USERS_POST_DuplicateFieldTest_FIRSTNAME",
            "lastName": "API_USERS_POST_DuplicateFieldTest_LASTNAME",
            "lastName": "API_USERS_POST_DuplicateFieldTest_LASTNAME",
            "middleName": "API_USERS_POST_DuplicateFieldTest_MIDDLENAME",
            "password": "API_USERS_POST_DuplicateFieldTest_PASSWORD",
            "schoolId": 1
        }
        """;
        context.Logger.LogDebug($"POST /api/users DuplicateFieldTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/users DuplicateFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }
}
