using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Listings.Photos;

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
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        request.Body =
        """
        {
            "condition": "Good",
            "purchaseType": "Buy",
            "price": "$19.99",
            "userId": 2,
            "isbn": "12345"
        }
        """;
        context.Logger.LogDebug($"POST /api/listings/photos SimpleTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/photos SimpleTest | response.Body = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        var entities = JsonSerializer.Deserialize<BookListingGetDto>(response.Body);
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
        request.Cookies = new string[] { "__Host-Http-UserId=2", "__Host-Http-SessionToken=AA==" };

        request.Body =
        """
        {
            "condition": "Good",
            "purchaseType": "Buy",
            "userId": 2,
            "price": "$19.99"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/photos MissingFieldTest | response.Body = {response.Body}");
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
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.Body =
        """
        {
            "condition": "Good",
            "purchaseType": "Buy",
            "price": "$19.99",
            "userId": 1,
            "isbn": "12349"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/photos DuplicateTest | response.Body 1 = {response.Body}");
        Assert.Equal(200, response.StatusCode);

        response = await function.FunctionHandler(request, context);

        context.Logger.LogDebug($"POST /api/listings/photos DuplicateTest | response.Body 2 = {response.Body}");
        Assert.Equal(200, response.StatusCode);
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
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.Body =
        """
        {
            "condition": "Good",
            "purchaseType": "Buy",
            "price": "$19.99",
            "userId": 1,
            "isbn": "12348",
            "location": "nowhere"
        }
        """;

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/photos MissingFieldTest | response.Body = {response.Body}");
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
        request.Cookies = new string[] { "__Host-Http-UserId=1", "__Host-Http-SessionToken=AA==" };

        request.Body =
        """
        {
            "condition": "Good",
            "purchaseType": "Buy",
            "price": "$19.99",
            "price": "$19.99",
            "userId": 1,
            "isbn": "12349"
        }
        """;
        context.Logger.LogDebug($"POST /api/listings/photos DuplicateFieldTest | request.Body = {request.Body}");

        var response = await function.FunctionHandler(request, context);

        Assert.NotNull(response);

        context.Logger.LogDebug($"POST /api/listings/photos DuplicateFieldTest | response.Body = {response.Body}");
        Assert.Equal(400, response.StatusCode);
    }
}
