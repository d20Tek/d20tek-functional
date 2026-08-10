namespace D20Tek.Functional.AspNetCore.MinimalApi;

/// <summary>
/// Extension methods on <see cref="IResultExtensions"/> for converting <see cref="Error"/> arrays
/// into RFC 7807 Problem Details responses in Minimal APIs.
/// </summary>
public static class TypedResultExtensions
{
    private const string _errorsExtensionName = "errors";

    /// <summary>
    /// Converts a collection of <see cref="Error"/> instances into an RFC 7807 Problem Details response.
    /// Validation errors produce a 400 ValidationProblem; other errors produce a standard Problem response.
    /// </summary>
    public static IResult Problem(this IResultExtensions _, IEnumerable<Error> errors) =>
        errors.Any() && errors.All(e => e.Type == ErrorType.Validation) ? ValidationProblem(errors) : Problem(errors);

    /// <summary>
    /// Converts a single <see cref="Error"/> into an RFC 7807 Problem Details response.
    /// </summary>
    public static IResult Problem(this IResultExtensions _, Error error) =>
        (error.Type == ErrorType.Validation)
            ? ValidationProblem([error])
            : Results.Problem(
                statusCode: MapErrorToCode(error),
                detail: error.Message,
                extensions: CreateErrorsExtension(error));

    /// <summary>
    /// Creates a Problem Details response with an explicit status code, error code, and message.
    /// </summary>
    public static IResult Problem(this IResultExtensions _, int statusCode, string errorCode, string message) =>
        Results.Problem(
            statusCode: statusCode,
            detail: message,
            extensions: CreateErrorsExtension(Error.Create(errorCode, message, statusCode)));

    private static IResult Problem(IEnumerable<Error> errors) =>
        errors.Any()
            ? errors.First().Pipe(error => Results.Problem(
                statusCode: MapErrorToCode(error),
                detail: error.Message,
                extensions: CreateErrorsExtension(errors)))
            : Results.Problem();

    private static int MapErrorToCode(Error error) => (int)ErrorTypeMapper.Instance.Convert(error.Type);

    private static Dictionary<string, object?> CreateErrorsExtension(IEnumerable<Error> errors) =>
        new()
        {
            { _errorsExtensionName, errors.ToDictionary(error => error.Code, error => error.Message) }
        };

    private static Dictionary<string, object?> CreateErrorsExtension(Error error) =>
        new()
        {
            { _errorsExtensionName, new Dictionary<string, string>() { { error.Code, error.Message } } }
        };

    private static IResult ValidationProblem(IEnumerable<Error> errors) =>
        Results.ValidationProblem(errors: errors.ToDictionary(error => error.Code, error => new[] { error.Message }));
}
