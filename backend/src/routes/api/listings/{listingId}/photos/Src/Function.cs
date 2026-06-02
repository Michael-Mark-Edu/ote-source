using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Annotations;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OTE.Common;
using OTE.Common.Api;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Dtos;
using OTE.Data.EFCore.Entities;
using System.Text;
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

        var listing = await oteContext
            .BookListings
            .Where(e => e.BookListingId == listingId)
            .Where(e => e.DeletedAt == null)
            .FirstOrDefaultAsync();

        if (listing == null)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 404,
                Body = $"{{\"error\":\"Listing '{listingId}' does not exist.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        int userId = listing.UserId;

        var validateCookieResult = await ApiFunctions.ValidateCookiesUserAction(request, oteContext, userId);

        if (validateCookieResult != null)
            return validateCookieResult;

        string fileExtension;
        try
        {
            switch (request.Headers["content-type"])
            {
                case "image/png":
                    fileExtension = ".png";
                    break;
                case "image/jpeg":
                    fileExtension = ".jpg";
                    break;
                default:
                    return new APIGatewayHttpApiV2ProxyResponse
                    {
                        StatusCode = 400,
                        Body = $"{{\"error\":\"Content-Type {request.Headers["content-type"]} invalid.\"}}",
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
            }
        }
        catch (NullReferenceException)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Header content-type expected but not given.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
        }
        catch (KeyNotFoundException)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Header content-type expected but not given.\"}}",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
        }

        byte[] data;
        if (request.IsBase64Encoded)
        {
            data = Convert.FromBase64String(request.Body);
        }
        else
        {
            data = Encoding.UTF8.GetBytes(request.Body);
        }

        var last = await oteContext
            .ListingPhotos
            .Where(e => e.BookListingId == listingId)
            .OrderByDescending(e => e.PhotoIndex)
            .FirstOrDefaultAsync();

        int nextIndex;
        if (last == null)
        {
            nextIndex = 1;
        }
        else
        {
            nextIndex = last.PhotoIndex + 1;
        }

        var dupe = await oteContext
            .ListingPhotos
            .Where(e => e.BookListingId == listingId)
            .Where(e => e.PhotoIndex == nextIndex)
            .FirstOrDefaultAsync();

        if (dupe != null)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Listing photo with index {nextIndex} already exists for listing {listingId}.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        var key = $"{listingId}-{nextIndex}{fileExtension}";

        var s3 = new AmazonS3Client(Amazon.RegionEndpoint.USWest2);
        using (var stream = new MemoryStream(data))
        {
            var response = await s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = "ote-listing-photos",
                Key = key,
                InputStream = stream
            });

            if ((int)response.HttpStatusCode != 200)
            {
                context.Logger.LogError("S3 upload did not return 200");
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 500,
                    Body = "Internal Server Error",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }

        var listingPhotoEntity = new ListingPhotoEntity
        {
            PhotoIndex = nextIndex,
            PhotoUrl = $"https://ote-listing-photos.s3.us-west-2.amazonaws.com/{key}",
            BookListingId = listingId
        };

        var insertedPhotoEntry = await oteContext
            .ListingPhotos
            .AddAsync(listingPhotoEntity);

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

        var insertedPhoto = insertedPhotoEntry.Entity;
        var photoGetDto = new ListingPhotoGetDto(insertedPhoto);
        var photoGetDtoJson = JsonSerializer.Serialize(photoGetDto);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = photoGetDtoJson,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
