using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using System.Text.Json;
using OTE.Common;
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
    private UserRepo _userRepo = null!;

    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        _factory = new OteContextFactory();
        _dbContext = _factory.CreateDbContext();
        _userRepo = new UserRepo(_dbContext);

        string userId = request.PathParameters["userId"];
        int parsedId;
        try
        {
            parsedId = int.Parse(userId);
        }
        catch (ArgumentNullException)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = $"Expected 32-bit signed integer at end of url, instead got null.",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                }
            };
        }
        catch (FormatException)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = $"Expected 32-bit signed integer at end of url, instead got '{userId}'.",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                }
            };
        }
        catch (OverflowException)
        {
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 400,
                Body = $"'{userId}' is out-of-range for a 32-bit signed integer.",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                }
            };
        }

        string method = request.RequestContext.Http.Method;

        switch (method)
        {
        case "GET":
            return await get(request, context, parsedId);
        default:
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 405,
                Body = $"Method \"{method}\" Not Allowed",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                    { "Allow", "GET" }
                }
            };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int userId)
    {
        var entityResult = await _userRepo.FindById(userId);
        if (!entityResult.Ok)
        {
            var error = entityResult.UnwrapError();
            var errorData = DatabaseErrorHandler.Parse(error);

            if (errorData.LogMessage != null)
                context.Logger.LogError($"UserRepo.FindById() error: {errorData.LogMessage}");

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = errorData.HttpStatus,
                Body = errorData.BodyMessage,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        var entity = entityResult.Unwrap();

        if (entity == null)
            return new APIGatewayHttpApiV2ProxyResponse {
                StatusCode = 404,
                Body = $"User with userId '{userId}' does not exist.",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

        var entityGetDto = new UserGetDto(entity);
        var entityJson = JsonSerializer.Serialize(entityGetDto);

        return new APIGatewayHttpApiV2ProxyResponse {
            StatusCode = 200,
            Body = entityJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
