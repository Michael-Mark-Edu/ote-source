using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OTE.Common;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Users.UserId;

public class Function
{
    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        string userId = request.PathParameters["userId"];
        var parsedIdResult = SafeAtoi.Parse(userId);
        if (!parsedIdResult.Ok)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"{parsedIdResult.UnwrapError().BodyMessage}\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" },
                }
            };

        int parsedId = parsedIdResult.Unwrap();

        string method = request.RequestContext.Http.Method;
        switch (method)
        {
            case "GET":
                return await get(request, context, parsedId);
            case "PATCH":
                return await patch(request, context, parsedId);
            case "DELETE":
                return await delete(request, context, parsedId);
            default:
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 405,
                    Body = $"{{\"error\":\"Method '{method}' not allowed.\"}}",
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                        { "Allow", "GET, PATCH, DELETE" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int userId)
    {
        var oteContext = OteContextSingleton.GetOrCreate();

        var foundUser = await oteContext
            .Users
            .Where(e => e.UserId == userId)
            .Where(e => e.DeletedAt == null)
            .FirstOrDefaultAsync();

        if (foundUser == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"User with userId '{userId}' does not exist.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
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

    private async Task<APIGatewayHttpApiV2ProxyResponse> patch(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int userId)
    {
        var oteContext = OteContextSingleton.GetOrCreate();

        var foundUserAsync = oteContext
            .Users
            .Where(e => e.UserId == userId)
            .Where(e => e.DeletedAt == null)
            .FirstOrDefaultAsync();

        var deserializeResult = ApiFunctions.DeserializeJson<Dictionary<string, JsonElement>>(request, context.Logger);
        if (!deserializeResult.Ok)
            return deserializeResult.UnwrapError();

        var dsz = deserializeResult.Unwrap();

        var foundUser = await foundUserAsync;

        if (foundUser == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"User with userId '{userId}' does not exist.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

        if (dsz.ContainsKey("username") && dsz["username"].ValueKind != JsonValueKind.String)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'username' to be {JsonValueKind.String}, instead got {dsz["username"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        if (dsz.ContainsKey("emailAddress") && dsz["emailAddress"].ValueKind != JsonValueKind.String)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'emailAddress' to be {JsonValueKind.String}, instead got {dsz["emailAddress"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        if (dsz.ContainsKey("firstName") && dsz["firstName"].ValueKind != JsonValueKind.String && dsz["firstName"].ValueKind == JsonValueKind.Null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'firstName' to be {JsonValueKind.String} or {JsonValueKind.Null}, instead got {dsz["firstName"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        if (dsz.ContainsKey("lastName") && dsz["lastName"].ValueKind != JsonValueKind.String && dsz["lastName"].ValueKind != JsonValueKind.Null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'lastName' to be {JsonValueKind.String} or {JsonValueKind.Null}, instead got {dsz["lastName"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        if (dsz.ContainsKey("middleName") && (dsz["middleName"].ValueKind == JsonValueKind.String || dsz["middleName"].ValueKind == JsonValueKind.Null))
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'middleName' to be {JsonValueKind.String} or {JsonValueKind.Null}, instead got {dsz["middleName"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        if (dsz.ContainsKey("password") && dsz["password"].ValueKind != JsonValueKind.String)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'password' to be {JsonValueKind.String}, instead got {dsz["password"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        if (dsz.ContainsKey("schoolId") && dsz["schoolId"].ValueKind != JsonValueKind.Number)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'schoolId' to be {JsonValueKind.Number}, instead got {dsz["schoolId"].ValueKind}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

        if (dsz.ContainsKey("username") && dsz["username"].ValueKind == JsonValueKind.String)
            foundUser.Username = dsz["username"]!.GetString()!;
        if (dsz.ContainsKey("emailAddress") && dsz["emailAddress"].ValueKind == JsonValueKind.String)
            foundUser.EmailAddress = dsz["emailAddress"]!.GetString()!;
        if (dsz.ContainsKey("firstName") && (dsz["firstName"].ValueKind == JsonValueKind.String || dsz["firstName"].ValueKind == JsonValueKind.Null))
            foundUser.FirstName = dsz["firstName"]!.GetString();
        if (dsz.ContainsKey("lastName") && (dsz["lastName"].ValueKind == JsonValueKind.String || dsz["lastName"].ValueKind == JsonValueKind.Null))
            foundUser.LastName = dsz["lastName"]!.GetString();
        if (dsz.ContainsKey("middleName") && (dsz["middleName"].ValueKind == JsonValueKind.String || dsz["middleName"].ValueKind == JsonValueKind.Null))
            foundUser.MiddleName = dsz["middleName"]!.GetString();

        int schoolId = 0;
        if (dsz.ContainsKey("schoolId") && dsz["schoolId"].ValueKind == JsonValueKind.Number && !dsz["schoolId"].TryGetInt32(out schoolId))
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Expected 'schoolId' to be a signed 32 bit integer, instead got a different {JsonValueKind.Number}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        else if (dsz.ContainsKey("schoolId") && dsz["schoolId"].ValueKind == JsonValueKind.Number)
            foundUser.SchoolId = schoolId;

        var updatedUserEntry = oteContext
            .Users
            .Update(foundUser);

        try
        {
            await oteContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                {
                    var n = (NpgsqlException)i;
                    return ApiFunctions.HandleRepoError(n, context.Logger);
                }
            }
            throw;
        }

        var updatedUser = updatedUserEntry.Entity;
        var userGetDto = new UserGetDto(updatedUser);
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
        var oteContext = OteContextSingleton.GetOrCreate();

        var foundUser = await oteContext
            .Users
            .Where(e => e.UserId == userId)
            .Where(e => e.DeletedAt == null)
            .FirstOrDefaultAsync();

        if (foundUser == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"User with userId '{userId}' does not exist.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

        foundUser.DeletedAt = DateTime.UtcNow;

        var updatedUserEntry = oteContext
            .Users
            .Update(foundUser);

        try
        {
            await oteContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            for (Exception? i = e; i != null; i = i.InnerException)
            {
                if (i.GetType().IsAssignableTo(typeof(NpgsqlException)))
                {
                    var n = (NpgsqlException)i;
                    return ApiFunctions.HandleRepoError(n, context.Logger);
                }
            }
            throw;
        }

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 204
        };
    }
}
