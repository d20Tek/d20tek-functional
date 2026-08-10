namespace D20Tek.Functional;

/// <summary>
/// Defines well-known numeric error type constants used by <see cref="Error"/>.
/// These values map to standard HTTP status code categories when used with the ASP.NET Core integrations.
/// You can extend this set with custom integer values via <see cref="Error.Create"/>.
/// </summary>
public static class ErrorType
{
    /// <summary>Unexpected/unhandled error (maps to HTTP 500).</summary>
    public const int Unexpected = 0;

    /// <summary>General operation failure (maps to HTTP 400).</summary>
    public const int Failure = 1;

    /// <summary>Input validation failure (maps to HTTP 400).</summary>
    public const int Validation = 2;

    /// <summary>Resource not found (maps to HTTP 404).</summary>
    public const int NotFound = 3;

    /// <summary>Resource conflict/duplicate (maps to HTTP 409).</summary>
    public const int Conflict = 4;

    /// <summary>Caller not authenticated (maps to HTTP 401).</summary>
    public const int Unauthorized = 5;

    /// <summary>Caller lacks permission (maps to HTTP 403).</summary>
    public const int Forbidden = 6;

    /// <summary>Semantically invalid request (maps to HTTP 422).</summary>
    public const int Invalid = 7;
}