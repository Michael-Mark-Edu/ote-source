using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Listings.Photos;

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

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        context.Logger.LogDebug($"GET /api/listings/photos SimpleTest | response.Body = {response.Body}");

        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<IEnumerable<BookListingGetDto>>(response.Body);
        Assert.NotNull(entities);
    }
}
