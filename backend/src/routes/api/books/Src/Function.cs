using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OTE.Routes.Api.Books;

public class Function
{
    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        string method;
        try
        {
            method = request.RequestContext.Http.Method;
        }
        catch (NullReferenceException)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request is not a valid AWS API Gateway HTTP API V2 request.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
        }

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
                    Body = $"{{\"error\":\"Method '{method}' not allowed.\"}}",
                    Headers = new Dictionary<string, string> {
                        { "Content-Type", "application/json" },
                        { "Allow", "GET, POST" }
                    }
                };
        }
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        using var oteContext = new OteContext();

        var books = await oteContext
            .Books
            .Where(e => e.DeletedAt == null)
            .ToListAsync();

        var bookGetDtos = books.Select((e, i) => new BookGetDto(e));
        var booksJson = JsonSerializer.Serialize(bookGetDtos);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = booksJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        using var oteContext = new OteContext();

        var deserializeResult = ApiFunctions.DeserializeJsonEntity<BookPostDto>(request, context.Logger);
        if (!deserializeResult.Ok)
            return deserializeResult.UnwrapError();

        var bookPostDtoOutput = deserializeResult.Unwrap().Map();

        var insertedBookEntry = await oteContext
            .Books
            .AddAsync(bookPostDtoOutput.BookEntity);

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

        var insertedBook = insertedBookEntry.Entity;
        var bookGetDto = new BookGetDto(insertedBook);
        var bookGetDtoJson = JsonSerializer.Serialize(bookGetDto);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = bookGetDtoJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
