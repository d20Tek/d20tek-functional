namespace D20Tek.Functional.AspNetCore;

/// <summary>
/// Singleton implementation of <see cref="IErrorTypeMapper"/> that maintains a global mapping
/// between <see cref="ErrorType"/> integer values and HTTP status codes.
/// Access the instance via <see cref="Instance"/> and reconfigure with <see cref="Configure"/>.
/// </summary>
public class ErrorTypeMapper : IErrorTypeMapper
{
    private static readonly Dictionary<int, HttpStatusCode> _map = [];

    private ErrorTypeMapper() { }

    /// <summary>
    /// Gets the singleton <see cref="IErrorTypeMapper"/> instance with default mappings applied.
    /// </summary>
    public static IErrorTypeMapper Instance { get; } = new ErrorTypeMapper().Configure();

    /// <inheritdoc/>
    public IErrorTypeMapper For(int errorType, HttpStatusCode statusCode) =>
        (_map[errorType] = statusCode).Pipe(_ => this);

    /// <inheritdoc/>
    public IErrorTypeMapper Remove(int errorType) => _map.Remove(errorType).Pipe(_ => this);

    /// <inheritdoc/>
    public bool Contains(int errorType) => _map.ContainsKey(errorType);

    /// <inheritdoc/>
    public HttpStatusCode Convert(int errorType) =>
        Contains(errorType) ? _map[errorType] : HttpStatusCode.InternalServerError;

    /// <inheritdoc/>
    public IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null) => 
        GetConfigurator(configure)
            .Iter(_ => _map.Clear())
            .Iter(c => c.Build().ForEach(entry => For(entry.ErrorType, entry.StatusCode)))
            .Pipe(_ => this);

    private static Identity<ErrorTypeConfigurator> GetConfigurator(Action<IErrorTypeConfigurator>? configure) =>
        new ErrorTypeConfigurator().ToIdentity()
                                   .Iter(configurator => configure?.Invoke(configurator));
}
