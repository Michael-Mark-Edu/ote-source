using Amazon.Lambda.APIGatewayEvents;
using Microsoft.EntityFrameworkCore;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Entities;

namespace OTE.Common.Api;

public static partial class ApiFunctions
{
    /// <summary>Parses cookies and determines if the user is either an admin or the target user.</summary>
    /// <param name="request">The `APIGatewayHttpApiV2ProxyRequest` to read the cookies of.</param>
    /// <param name="oteContext">The `OteContext` to read user and session token data from.</param>
    /// <param name="userId">The `UserId` to check against.</param>
    /// <returns>`null` if the user is validated, otherwise returns a 4xx HTTP response ready to be returned.</returns>
    public static async Task<APIGatewayHttpApiV2ProxyResponse?> ValidateCookiesUserAction(APIGatewayHttpApiV2ProxyRequest request, OteContext oteContext, int userId)
    {
        var validateCookieResult = await _validateCookies(request, oteContext);

        if (!validateCookieResult.Ok)
            return validateCookieResult.UnwrapError();

        var dbSessionToken = validateCookieResult.Unwrap();

        if (dbSessionToken.UserId != userId && !dbSessionToken.User.IsAdmin)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 403,
                Body = $"{{\"error\":\"Insufficient priveleges.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

        return null;
    }

    /// <summary>Parses cookies and determines if the user is an admin.</summary>
    /// <param name="request">The `APIGatewayHttpApiV2ProxyRequest` to read the cookies of.</param>
    /// <param name="oteContext">The `OteContext` to read user and session token data from.</param>
    /// <returns>`null` if the user is validated, otherwise returns a 4xx HTTP response ready to be returned.</returns>
    public static async Task<APIGatewayHttpApiV2ProxyResponse?> ValidateCookiesAdminAction(APIGatewayHttpApiV2ProxyRequest request, OteContext oteContext)
    {
        var validateCookieResult = await _validateCookies(request, oteContext);

        if (!validateCookieResult.Ok)
            return validateCookieResult.UnwrapError();

        var dbSessionToken = validateCookieResult.Unwrap();

        if (!dbSessionToken.User.IsAdmin)
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 403,
                Body = $"{{\"error\":\"Insufficient priveleges.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

        return null;
    }

    private static async Task<Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>> _validateCookies(APIGatewayHttpApiV2ProxyRequest request, OteContext oteContext)
    {
        if (request.Cookies == null)
            return new Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 401,
                Body = $"{{\"error\":\"Cookies '__Host-Http-UserId' and '__Host-Http-SessionToken' must be specified.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });

        var sessionTokenUserIdCookie = request
            .Cookies
            .Where(c => c.Length >= 19 && c.Substring(0, 19) == "__Host-Http-UserId=")
            .FirstOrDefault();

        if (sessionTokenUserIdCookie == null)
            return new Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 401,
                Body = $"{{\"error\":\"Cookie '__Host-Http-UserId' must be specified.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });

        var sessionTokenUserId = sessionTokenUserIdCookie.Substring(19);

        var sessionTokenUserIdParseResult = SafeAtoi.Parse(sessionTokenUserId);
        if (!sessionTokenUserIdParseResult.Ok)
            return new Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Could not parse '__Host-Http-UserId' into an integer.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });

        var sessionTokenUserIdParse = sessionTokenUserIdParseResult.Unwrap();

        var sessionTokenDataCookie = request
            .Cookies
            .Where(c => c.Length >= 25 && c.Substring(0, 25) == "__Host-Http-SessionToken=")
            .FirstOrDefault();

        if (sessionTokenDataCookie == null)
            return new Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 401,
                Body = $"{{\"error\":\"Cookie '__Host-Http-SessionToken' must be specified.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });

        var sessionTokenData = sessionTokenDataCookie.Substring(25);

        byte[] sessionTokenDataBytes = Convert.FromBase64String(sessionTokenData);

        var dbSessionToken = await oteContext
            .SessionTokens
            .Include(e => e.User)
            .Where(e => e.Token == sessionTokenDataBytes)
            .SingleOrDefaultAsync();

        if (dbSessionToken == null)
        {
            return new Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 403,
                Body = $"{{\"error\":\"Insufficient priveleges.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }

        return new Result<SessionTokenCacheEntity, APIGatewayHttpApiV2ProxyResponse>(dbSessionToken);
    }
}
