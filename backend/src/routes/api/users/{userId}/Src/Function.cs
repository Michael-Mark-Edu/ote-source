using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using System.Text.Json;
using OTE.Common;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Factories;
using OTE.Data.EFCore.Repositories;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Users.UserId;

public class Function
{
    private OteContextFactory _factory = null!;
    private OteContext _dbContext = null!;

    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();

        string userId = request.PathParameters["userId"];
        var parsedIdResult = SafeAtoi.Parse(userId);
        if (!parsedIdResult.Ok)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = parsedIdResult.UnwrapError().BodyMessage,
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                }
            };

        int parsedId = parsedIdResult.Unwrap();

        string method = request.RequestContext.Http.Method;
        switch (method)
        {
            case "GET":
                return await get(request, context, parsedId);
            case "DELETE":
                return await delete(request, context, parsedId);
            default:
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 405,
                    Body = $"Method \"{method}\" Not Allowed",
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "text/plain" },
                        { "Allow", "GET, DELETE" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int userId)
    {
        var userRepo = new UserRepo(_dbContext);

        var findUserResult = await userRepo.FindById(userId);
        if (!findUserResult.Ok)
            return ApiFunctions.HandleRepoError(findUserResult.UnwrapError(), context.Logger);

        var foundUser = findUserResult.Unwrap();

        if (foundUser == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"User with userId '{userId}' does not exist.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

        var userGetDto = new UserGetDto(foundUser);
        var userGetDtoJson = JsonSerializer.Serialize(userGetDto);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = userGetDtoJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> delete(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int userId)
    {
        var userRepo = new UserRepo(_dbContext);

        var entityResult = await userRepo.Delete(userId);
        if (!entityResult.Ok)
            return ApiFunctions.HandleRepoError(entityResult.UnwrapError(), context.Logger);

        var entity = entityResult.Unwrap();

        if (entity == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"User with userId '{userId}' does not exist.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 204
        };
    }
}
