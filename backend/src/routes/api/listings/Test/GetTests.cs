using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Listings;

public class GetTests
{
    [Fact]
    public async Task SimpleTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings SimpleTest | response.Body = {response.Body}");

        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<IEnumerable<UserGetDto>>(response.Body);
        Assert.NotNull(entities);
    }

    [Fact]
    public async Task IllegalCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";
        request.Cookies = new string[] { "__Host-Http-UserId=0", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings IllegalCookiesTest | response.Body = {response.Body}");

        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task NonAdminCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings NonAdminCookiesTest | response.Body = {response.Body}");

        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task NoCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings NoCookiesTest | response.Body = {response.Body}");

        Assert.Equal(401, response.StatusCode);
    }
}
