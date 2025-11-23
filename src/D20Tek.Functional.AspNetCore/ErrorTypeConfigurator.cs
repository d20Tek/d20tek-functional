namespace D20Tek.Functional.AspNetCore;

internal sealed class ErrorTypeConfigurator : IErrorTypeConfigurator
{
    private readonly Dictionary<int, HttpStatusCode> _config = new()
    {
        { ErrorType.Unexpected, HttpStatusCode.InternalServerError },
        { ErrorType.Conflict, HttpStatusCode.Conflict },
        { ErrorType.Validation, HttpStatusCode.BadRequest },
        { ErrorType.Failure, HttpStatusCode.BadRequest },
        { ErrorType.NotFound, HttpStatusCode.NotFound },
        { ErrorType.Unauthorized, HttpStatusCode.Unauthorized },
        { ErrorType.Forbidden, HttpStatusCode.Forbidden },
        { ErrorType.Invalid, HttpStatusCode.UnprocessableEntity }
    };

    public IErrorTypeConfigurator For(int errorType, HttpStatusCode statusCode) =>
        (_config[errorType] = statusCode).Pipe(_ => this);

    public IErrorTypeConfigurator Remove(int errorType) => _config.Remove(errorType).Pipe(_ =>  this);

    public IErrorTypeConfigurator Clear() => _config.Pipe(c => c.Clear()).Pipe(_ => this);

    internal IList<ConfigEntry> Build() => [.. _config.Select(x => new ConfigEntry(x.Key, x.Value))];
}

internal sealed record ConfigEntry(int ErrorType, HttpStatusCode StatusCode);