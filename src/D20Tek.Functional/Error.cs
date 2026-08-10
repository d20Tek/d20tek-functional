namespace D20Tek.Functional;

/// <summary>
/// Represents a structured error with a type classification, code identifier, and human-readable message.
/// Use this value type to convey domain errors through <see cref="Result{T}"/> without throwing exceptions,
/// enabling railway-oriented programming patterns.
/// </summary>
public readonly struct Error
{
    /// <summary>
    /// Gets the numeric error type classification (see <see cref="ErrorType"/> constants).
    /// This value is used by error-to-HTTP-status-code mappers in ASP.NET Core integrations or other mappings you may implement.
    /// </summary>
    public int Type { get; }

    /// <summary>
    /// Gets the machine-readable error code that uniquely identifies this error category
    /// (e.g., "User.NotFound", "Validation.Email").
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the human-readable error message describing what went wrong.
    /// </summary>
    public string Message { get; }

    private Error(string code, string message, int errorType) => (Type, Code, Message) = (errorType, code, message);

    /// <summary>
    /// Creates an error representing an unexpected/unhandled failure (HTTP 500 equivalent).
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the error.</param>
    public static Error Unexpected(string code, string message) => new(code, message, ErrorType.Unexpected);

    /// <summary>
    /// Creates an error representing a general operation failure (HTTP 400 equivalent).
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the error.</param>
    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    /// <summary>
    /// Creates an error representing a validation failure (HTTP 400 equivalent).
    /// Use this for input validation errors that should be reported back to the caller.
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the validation failure.</param>
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    /// <summary>
    /// Creates an error indicating a requested resource was not found (HTTP 404 equivalent).
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of what was not found.</param>
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    /// <summary>
    /// Creates an error indicating a resource conflict (HTTP 409 equivalent),
    /// such as attempting to create a duplicate entity.
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the conflict.</param>
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    /// <summary>
    /// Creates an error indicating the caller is not authenticated (HTTP 401 equivalent).
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the authorization failure.</param>
    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);

    /// <summary>
    /// Creates an error indicating the caller lacks permission (HTTP 403 equivalent).
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the forbidden action.</param>
    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);

    /// <summary>
    /// Creates an error indicating the request is semantically invalid (HTTP 422 equivalent).
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of why the input is invalid.</param>
    public static Error Invalid(string code, string message) => new(code, message, ErrorType.Invalid);

    /// <summary>
    /// Creates an <see cref="Error"/> from a caught <see cref="System.Exception"/>,
    /// using the exception message and classifying it as <see cref="ErrorType.Unexpected"/>.
    /// </summary>
    /// <param name="ex">The exception to convert into an error.</param>
    public static Error Exception(Exception ex) => new(Constants.GeneralExceptionCode, ex.Message, ErrorType.Unexpected);

    /// <summary>
    /// Creates an error with a custom numeric error type. Use this when the built-in
    /// <see cref="ErrorType"/> constants do not cover your domain-specific classification.
    /// </summary>
    /// <param name="code">A machine-readable error code.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <param name="errorType">A custom numeric classification for this error.</param>
    public static Error Create(string code, string message, int errorType) => new(code, message, errorType);

    /// <inheritdoc/>
    public override string ToString() => $"Error ({Code} [{Type}]): {Message}";
}