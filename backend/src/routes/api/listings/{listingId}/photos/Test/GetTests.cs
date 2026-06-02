using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Listings.ListingId.Photos;

public class GetTests
{
    [Fact]
    public async Task SimpleTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "listingId", "1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings/{{listingId}}/photos SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<BookListingGetDto>(response.Body);
        Assert.NotNull(entities);
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
        request.RequestContext.Http.Method = "GET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings/{{listingId}}/photos NegativeIdTest | response.Body = {response.Body}");
        Assert.Equal(404, response.StatusCode);
    }
}
