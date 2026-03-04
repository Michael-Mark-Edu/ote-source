using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace OTE.Routes.Api.Listings.ListingId;

public class DeleteTests
{
    [Fact]
    public async Task UserSelfDeleteTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "5" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/listings/{{listingId}} UserSelfDeleteTest | response.Body = {response.Body}");
        Assert.Equal(204, response.StatusCode);

        request.RequestContext.Http.Method = "GET";

        response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task AdminDeleteTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "4" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/listings/{{listingId}} AdminDeleteTest | response.Body = {response.Body}");
        Assert.Equal(204, response.StatusCode);

        request.RequestContext.Http.Method = "GET";

        response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task IllegalCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "3" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=0", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/listings/{{listingId}} IllegalCookiesTest | response.Body = {response.Body}");
        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task WrongCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "3" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=3", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/listings/{{listingId}} WrongCookiesTest | response.Body = {response.Body}");
        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task NoCookiesTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "3" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/listings/{{listingId}} NoCookiesTest | response.Body = {response.Body}");
        Assert.Equal(401, response.StatusCode);
    }

    [Fact]
    public async Task NegativeIdTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "-1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";
        request.Cookies = new string[] { "__Host-Http-UserId=6", "__Host-Http-SessionToken=AA==" };

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"DELETE /api/listings/{{listingId}} NegativeIdTest | response.Body = {response.Body}");
        Assert.Equal(404, response.StatusCode);
    }
}
