using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Entities;
using Xunit;

namespace OTE.Routes.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestFunction()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayProxyRequest();
        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(200, response.StatusCode);

        try
        {
            var entities = JsonSerializer.Deserialize<IEnumerable<SchoolEntity>>(response.Body);
            Assert.NotNull(entities);

            Console.WriteLine(response.Body);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }

    }
}
