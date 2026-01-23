using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using System.Text.Json;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Factories;
using OTE.Data.EFCore.Repositories;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Users;

public class Function
{
    private OteContextFactory _factory = null!;
    private OteContext _dbContext = null!;

    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();

        string method = request.RequestContext.Http.Method;

        switch (method)
        {
            case "GET":
                return await get(request, context);
            case "POST":
                return await post(request, context);
            default:
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 405,
                    Body = $"Method \"{method}\" not allowed.",
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "text/plain" },
                        { "Allow", "GET, POST" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var userRepo = new UserRepo(_dbContext);
        var usersResult = await userRepo.GetAll();

        if (!usersResult.Ok)
            return ApiFunctions.HandleRepoError(usersResult.UnwrapError(), context.Logger);

        var users = usersResult.Unwrap();
        var userGetDtos = users.Select((e, i) => new UserGetDto(e));
        var usersJson = JsonSerializer.Serialize(userGetDtos);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = usersJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var userRepo = new UserRepo(_dbContext);
        var passwordRepo = new Argon2idPasswordRepo(_dbContext);

        var deserializeResult = ApiFunctions.DeserializeJson<UserPostDto>(request, context.Logger);
        if (!deserializeResult.Ok)
            return deserializeResult.UnwrapError();

        var userPostDtoOutput = deserializeResult.Unwrap().Map();

        var insertPasswordResult = await passwordRepo.Insert(userPostDtoOutput.Argon2idPasswordEntity);
        if (!insertPasswordResult.Ok)
            return ApiFunctions.HandleRepoError(insertPasswordResult.UnwrapError(), context.Logger);

        var insertUserResult = await userRepo.Insert(userPostDtoOutput.UserEntity);
        if (!insertUserResult.Ok)
            return ApiFunctions.HandleRepoError(insertUserResult.UnwrapError(), context.Logger);

        var insertedUserEntry = insertUserResult.Unwrap();
        var insertedUser = insertedUserEntry.Entity;
        var userGetDto = new UserGetDto(insertedUser);
        var userGetDtoJson = JsonSerializer.Serialize(userGetDto);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = userGetDtoJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
