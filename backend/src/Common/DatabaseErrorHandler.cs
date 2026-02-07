using Npgsql;

namespace OTE.Common;

/// <summary>Struct containing HTTP response information.</summary>
public class DatabaseErrorData
{
    public int HttpStatus { get; set; } = 500;
    public string BodyMessage { get; set; } = "Internal Server Error";
    public string? LogMessage { get; set; } = "Unknown error occured";
}

/// <summary>Static class for getting HTTP error information from an `NpgsqlException`.</summary>
public static class DatabaseErrorHandler
{
    /// <summary>Gets HTTP error information from an `NpgsqlException`.</summary>
    /// <param name="ex">`NpgsqlException` to parse.</param>
    /// <returns>A `DatabaseErrorData` instance containing relevant error information.</returns>
    public static DatabaseErrorData Parse(NpgsqlException ex)
    {
        switch (ex)
        {
            case PostgresException pex:
                switch (pex.SqlState)
                {
                    case PostgresErrorCodes.NotNullViolation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 400,
                            BodyMessage = $"Column {pex.ColumnName} of table {pex.TableName} cannot be null.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.ForeignKeyViolation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 400,
                            BodyMessage = $"Foreign key constraint violation with constraint {pex.ConstraintName} of table {pex.TableName}.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.UniqueViolation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 400,
                            BodyMessage = $"Duplicate value violates the unique constraint {pex.ConstraintName} of table {pex.TableName}.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.CheckViolation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 400,
                            BodyMessage = $"Check constraint violation on column {pex.ColumnName} in table {pex.TableName}.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.ExclusionViolation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 400,
                            BodyMessage = $"Exclusion constraint violation occurred.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.StringDataRightTruncation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 400,
                            BodyMessage = $"A value is too long.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.SyntaxError:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 500,
                            BodyMessage = $"Internal Server Error",
                            LogMessage = "Syntax error occured while processing the request."
                        };
                    case PostgresErrorCodes.LockNotAvailable:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 500,
                            BodyMessage = $"Internal Server Error",
                            LogMessage = "Database lock not available."
                        };
                    case PostgresErrorCodes.DeadlockDetected:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 500,
                            BodyMessage = $"Internal Server Error",
                            LogMessage = "Deadlock detected in the database."
                        };
                    case PostgresErrorCodes.InsufficientPrivilege:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 403,
                            BodyMessage = $"Insufficient privileges to perform the operation.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.InvalidAuthorizationSpecification:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 401,
                            BodyMessage = $"Invalid database credentials provided.",
                            LogMessage = null
                        };
                    case PostgresErrorCodes.ConnectionException:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 500,
                            BodyMessage = $"Internal Server Error",
                            LogMessage = "Database connection exception."
                        };
                    case PostgresErrorCodes.ProtocolViolation:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 500,
                            BodyMessage = $"Internal Server Error",
                            LogMessage = "Protocol violation with database communication."
                        };
                    default:
                        return new DatabaseErrorData
                        {
                            HttpStatus = 500,
                            BodyMessage = $"Internal Server Error",
                            LogMessage = $"Unhandled PostgreSQL error: {pex.SqlState}"
                        };
                }

            default:
                if (ex.InnerException != null)
                {
                    switch (ex.InnerException)
                    {
                        case IOException io:
                            return new DatabaseErrorData
                            {
                                HttpStatus = 500,
                                BodyMessage = "Internal Server Error",
                                LogMessage = $"IOException: {io.Message}"
                            };
                        default:
                            return new DatabaseErrorData
                            {
                                HttpStatus = 500,
                                BodyMessage = "Internal Server Error",
                                LogMessage = $"Uncaught exception {ex.GetType().Name}: {ex.InnerException.Message}"
                            };
                    }
                }
                else
                {
                    return new DatabaseErrorData
                    {
                        HttpStatus = 500,
                        BodyMessage = "Internal Server Error",
                        LogMessage = $"Uncaught NpgsqlException: {ex.Message}"
                    };
                }
        }
    }
}
