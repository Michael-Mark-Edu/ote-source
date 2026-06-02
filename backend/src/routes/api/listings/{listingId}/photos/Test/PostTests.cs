using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Listings.ListingId.Photos;

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
        request.Headers = new Dictionary<string, string> { { "Content-Type", "image/png" } };
        request.PathParameters = new Dictionary<string, string> { { "listingId", "1" } };
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.Body = "AA==";
        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos SimpleTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<ListingPhotoGetDto>(response.Body);
        Assert.NotNull(entities);
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
        request.Headers = new Dictionary<string, string> { { "Content-Type", "image/png" } };
        request.PathParameters = new Dictionary<string, string> { { "listingId", "1" } };
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.Body = "AA==";

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos DuplicateTest | response.Body 1 = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        response = await function.FunctionHandler(request, context);

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos DuplicateTest | response.Body 2 = {response.Body}");
        Assert.Equal(200, response.StatusCode);
    }

    [Fact]
    public async Task WrongUserTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";
        request.Headers = new Dictionary<string, string> { { "Content-Type", "image/png" } };
        request.PathParameters = new Dictionary<string, string> { { "listingId", "1" } };
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        request.Body = "AA==";

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos WrongUserTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos WrongUserTest | response.Body = {response.Body}");
        Assert.Equal(403, response.StatusCode);
    }

    [Fact]
    public async Task IllegalFileTypeTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";
        request.Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
        request.PathParameters = new Dictionary<string, string> { { "listingId", "1" } };
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.Body = "AA==";

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos IllegalFileTypeTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/{{listingId}}/photos IllegalFileTypeTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }
}
