namespace D20Tek.Functional.AspNetCore.MinimalApi;

public static class TypedResultExtensions
{
    private const string _errorsExtensionName = "errors";

    public static IResult Problem(this IResultExtensions _, IEnumerable<Error> errors) =>
        errors.Any() && errors.All(e => e.Type == ErrorType.Validation) ? ValidationProblem(errors) : Problem(errors);

    public static IResult Problem(this IResultExtensions _, Error error) =>
        (error.Type == ErrorType.Validation)
            ? ValidationProblem([error])
            : Results.Problem(
                statusCode: MapErrorToCode(error),
                detail: error.Message,
                extensions: CreateErrorsExtension(error));

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
