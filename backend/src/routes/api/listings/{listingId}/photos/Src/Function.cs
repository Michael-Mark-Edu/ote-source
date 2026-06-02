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

namespace OTE.Routes.Api.Listings.ListingId.Photos;

public class Function
{
    [LambdaFunction]
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        string listingId;
        try
        {
            listingId = request.PathParameters["listingId"];
        }
        catch (NullReferenceException)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"listingId expected but not given.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
        }

        var parsedIdResult = SafeAtoi.Parse(listingId);
        if (!parsedIdResult.Ok)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"{parsedIdResult.UnwrapError()}\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" },
                }
            };

        int parsedId = parsedIdResult.Unwrap();

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
                return await get(request, context, parsedId);
            case "POST":
                return await post(request, context, parsedId);
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

    private async Task<APIGatewayHttpApiV2ProxyResponse> get(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int listingId)
    {
        using var oteContext = new OteContext();

        var photos = await oteContext
            .ListingPhotos
            .Where(e => e.BookListingId == listingId)
            .Where(e => e.DeletedAt == null)
            .ToArrayAsync();

        if (photos.Count() <= 0)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"Listing '{listingId}' does not have any photos.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        List<ListingPhotoGetDto> photoGetDtos = new();
        foreach (var photo in photos)
        {
            photoGetDtos.Add(new ListingPhotoGetDto(photo));
        }

        var photoGetDtosJson = JsonSerializer.Serialize(photoGetDtos);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = photoGetDtosJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    private async Task<APIGatewayHttpApiV2ProxyResponse> post(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context, int listingId)
    {
        using var oteContext = new OteContext();

        var foundListing = await oteContext
            .BookListings
            .Include(e => e.Seller)
            .Where(e => e.BookListingId == listingId)
            .Where(e => e.DeletedAt == null)
            .FirstOrDefaultAsync();

        if (foundListing == null)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"Listing with listingId '{listingId}' does not exist.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

        var validateCookieResult = await ApiFunctions.ValidateCookiesUserAction(request, oteContext, foundListing.UserId);

        if (validateCookieResult != null)
            return validateCookieResult;

        foundListing.DeletedAt = DateTime.UtcNow;

        var updatedListingEntry = oteContext
            .BookListings
            .Update(foundListing);

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
