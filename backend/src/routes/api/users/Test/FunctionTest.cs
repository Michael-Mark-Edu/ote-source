using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Entities;
using Xunit;

namespace OTE.Routes.Api.Users;

[Collection("Lambda Tests")]
public class FunctionTest
{
    [Fact]
    public async Task GetTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "GET";

        var response = await function.FunctionHandler(request, context);
        Assert.NotNull(response);

        Assert.Equal(200, response.StatusCode);

        try
        {
            var entities = JsonSerializer.Deserialize<IEnumerable<UserGetDto>>(response.Body);
            Assert.NotNull(entities);

            Console.WriteLine(response.Body);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [Fact]
    public async Task PostTest()
    {
        var function = new Function();
        var context = new TestLambdaContext();
        var request = new APIGatewayHttpApiV2ProxyRequest();
        request.RequestContext = new();
        request.RequestContext.Http = new();
        request.RequestContext.Http.Method = "POST";

        request.Body = JsonSerializer.Serialize(new UserPostDto
        {
            Username = "_TEST_USERNAME",
            EmailAddress = "_TEST_EMAIL",
            FirstName = "_TEST_FIRST_NAME",
            LastName = "_TEST_LAST_NAME",
            MiddleName = "_TEST_MIDDLE_NAME",
            Password = "_TEST_PASSWORD",
            SchoolId = 1
        });

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
}
