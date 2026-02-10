using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Entities;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OTE.Common.Api;

/// <summary>Static class for encapsulating repetitive and verbose code segments.</summary>
public static class ApiFunctions
{
    /// <summary>Gets an HTTP response from an `NpgsqlException` while also handling logging.</summary>
    /// <param name="error">The `NpgsqlException` to parse.</param>
    /// <param name="logger">The `ILambdaLogger` instance used for logging.</param>
    /// <returns>An `APIGatewayHttpApiV2ProxyResponse` ready to be sent to the user.</returns>
    public static APIGatewayHttpApiV2ProxyResponse HandleRepoError(NpgsqlException error, ILambdaLogger logger)
    {
        var errorData = DatabaseErrorHandler.Parse(error);

        if (errorData.LogMessage != null)
            logger.LogError($"Argon2idPasswordRepo.Insert() error: {errorData.LogMessage}");

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = errorData.HttpStatus,
            Body = $"{{\"error\":\"{errorData.BodyMessage}\"}}",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    /// <summary>Parses HTTP request body data into a generic object.</summary>
    /// <param name="request">The `APIGatewayHttpApiV2ProxyRequest` to read the body of for parsing.</param>
    /// <param name="logger">The `ILambdaLogger` instance used for logging.</param>
    /// <typeparam name="TTarget">A constructable type that is created from JSON data.</typeparam>
    /// <returns>A `Result` type containing either a new `TTarget` instance, or a `APIGatewayHttpApiV2ProxyResponse` ready to be sent to the user.</returns>
    public static Result<TTarget, APIGatewayHttpApiV2ProxyResponse> DeserializeJsonEntity<TTarget>(APIGatewayHttpApiV2ProxyRequest request, ILambdaLogger logger)
        where TTarget : new()
    {
        TTarget target = new TTarget();
        try
        {
            string body;
            if (request.IsBase64Encoded)
                body = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.Body));
            else
                body = request.Body;

            var json = JsonDocument.Parse(body);

            Dictionary<string, PropertyInfo> jsonMappings = new();
            Dictionary<string, PropertyInfo> exhaustedMappings = new();
            foreach (var p in typeof(TTarget).GetProperties())
            {
                JsonPropertyNameAttribute? attr = (JsonPropertyNameAttribute?)p.GetCustomAttribute(typeof(JsonPropertyNameAttribute));
                if (attr != null)
                {
                    jsonMappings[attr.Name] = p;
                }
            }

            foreach (var e in json.RootElement.EnumerateObject())
            {
                if (!jsonMappings.ContainsKey(e.Name))
                    if (exhaustedMappings.ContainsKey(e.Name))
                        return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                        {
                            StatusCode = 400,
                            Body = $"{{\"error\":\"JSON field {e.Name} is duplicated.\"}}",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        });
                    else
                        return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                        {
                            StatusCode = 400,
                            Body = $"{{\"error\":\"JSON field {e.Name} could not be mapped to database.\"}}",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        });

                var prop = jsonMappings[e.Name];

                switch (e.Value.ValueKind)
                {
                    case JsonValueKind.Null:
                        if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                            {
                                StatusCode = 400,
                                Body = $"{{\"error\":\"JSON field {e.Name} cannot be null.\"}}",
                                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                            });
                        prop.SetValue(target, null);
                        break;
                    case JsonValueKind.Number:
                        switch (prop.PropertyType)
                        {
                            case Type t when t == typeof(sbyte):
                                prop.SetValue(target, e.Value.GetSByte());
                                break;
                            case Type t when t == typeof(byte):
                                prop.SetValue(target, e.Value.GetByte());
                                break;
                            case Type t when t == typeof(short):
                                prop.SetValue(target, e.Value.GetInt16());
                                break;
                            case Type t when t == typeof(ushort):
                                prop.SetValue(target, e.Value.GetUInt16());
                                break;
                            case Type t when t == typeof(int):
                                prop.SetValue(target, e.Value.GetInt32());
                                break;
                            case Type t when t == typeof(uint):
                                prop.SetValue(target, e.Value.GetUInt32());
                                break;
                            case Type t when t == typeof(long):
                                prop.SetValue(target, e.Value.GetInt64());
                                break;
                            case Type t when t == typeof(ulong):
                                prop.SetValue(target, e.Value.GetUInt64());
                                break;
                            case Type t when t == typeof(float) || t == typeof(double):
                                prop.SetValue(target, e.Value.GetDouble());
                                break;
                            case Type t when t == typeof(decimal):
                                prop.SetValue(target, e.Value.GetDecimal());
                                break;
                            default:
                                return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                                {
                                    StatusCode = 400,
                                    Body = $"{{\"error\":\"JSON field {e.Name} must be a number.\"}}",
                                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                                });
                        }
                        break;
                    case JsonValueKind.String:
                        if (!prop.PropertyType.IsAssignableTo(typeof(string)))
                            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                            {
                                StatusCode = 400,
                                Body = $"{{\"error\":\"JSON field {e.Name} must be a string.\"}}",
                                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                            });
                        prop.SetValue(target, e.Value.GetString());
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        if (!prop.PropertyType.IsAssignableTo(typeof(bool)))
                            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                            {
                                StatusCode = 400,
                                Body = $"{{\"error\":\"JSON field {e.Name} must be a bool.\"}}",
                                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                            });
                        prop.SetValue(target, e.Value.GetBoolean());
                        break;
                    default:
                        return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                        {
                            StatusCode = 400,
                            Body = $"{{\"error\":\"JSON field {e.Name} has bad type.\"}}",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        });
                }

                exhaustedMappings.Add(e.Name, jsonMappings[e.Name]);
                jsonMappings.Remove(e.Name);
            }

            if (jsonMappings.Count > 0)
            {
                return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 400,
                    Body = $"{{\"error\":\"JSON data contains {jsonMappings.Count} extra field{(jsonMappings.Count == 1 ? "" : "s")}.\"}}",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                });
            }
        }
        catch (JsonException e)
        {
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body contains invalid JSON data. ${e.Message}\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (ArgumentNullException)
        {
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body must contain JSON data of the entity to insert.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (Exception e)
        {
            logger.LogError($"Unknown exception occured on line: {e.Message} {e.StackTrace}");
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 500,
                Body = $"{{\"error\":\"Internal server error.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        if (target == null)
        {
            return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Failed to deserialize request body.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }

        return Result<TTarget, APIGatewayHttpApiV2ProxyResponse>.NewOk(target);
    }

    /// <summary>Parses HTTP request body data into a `string`-`JsonElement` dictionary.</summary>
    /// <param name="request">The `APIGatewayHttpApiV2ProxyRequest` to read the body of for parsing.</param>
    /// <param name="logger">The `ILambdaLogger` instance used for logging.</param>
    /// <returns>A `Result` type containing either a new `Dictionary` instance, or a `APIGatewayHttpApiV2ProxyResponse` ready to be sent to the user.</returns>
    public static Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse> DeserializeJsonDictionary(APIGatewayHttpApiV2ProxyRequest request, ILambdaLogger logger)
    {
        Dictionary<string, JsonElement> target = new();
        try
        {
            string body;
            if (request.IsBase64Encoded)
                body = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.Body));
            else
                body = request.Body;

            var json = JsonDocument.Parse(body);

            foreach (var e in json.RootElement.EnumerateObject())
                target.Add(e.Name, e.Value);
        }
        catch (JsonException e)
        {
            return Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body contains invalid JSON data. ${e.Message}\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (ArgumentNullException)
        {
            return Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body must contain JSON data of the entity to insert.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (ArgumentException)
        {
            return Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Request body contains a duplicated JSON field.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        catch (Exception e)
        {
            logger.LogError($"Unknown exception occured on line: {e.Message} {e.StackTrace}");
            return Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 500,
                Body = $"{{\"error\":\"Internal server error.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }
        if (target == null)
        {
            return Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse>.NewError(new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 400,
                Body = $"{{\"error\":\"Failed to deserialize request body.\"}}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            });
        }

        return Result<Dictionary<string, JsonElement>, APIGatewayHttpApiV2ProxyResponse>.NewOk(target);
    }

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
