using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using Xunit;

namespace OTE.Routes.Api.Users.UserId;

[Collection("Lambda Tests")]
public class FunctionTest
{
    [Fact]
    public async Task GetTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "1" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        if (response.StatusCode != 200)
        {
            Console.WriteLine(response.Body);
            Assert.Equal(200, response.StatusCode);
        }

        try
        {
            var entities = JsonSerializer.Deserialize<UserGetDto>(response.Body);
            Assert.NotNull(entities);

            Console.WriteLine(response.Body);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [Fact]
    public async Task DeleteTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.PathParameters = new Dictionary<string, string> { { "userId", "2" } };
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "DELETE";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        if (response.StatusCode != 204)
        {
            Console.WriteLine(response.Body);
            Assert.Equal(204, response.StatusCode);
        }
    }
}
