using System.Net;

namespace D20Tek.Functional.AspNetCore;

public class ErrorTypeMapper : IErrorTypeMapper
{
    private static readonly Dictionary<int, HttpStatusCode> _map = [];

    private ErrorTypeMapper() { }

    public static IErrorTypeMapper Instance { get; } = new ErrorTypeMapper().Configure();

    public IErrorTypeMapper For(int errorType, HttpStatusCode statusCode) =>
        (_map[errorType] = statusCode).Pipe(_ => this);

    public IErrorTypeMapper Remove(int errorType) => _map.Remove(errorType).Pipe(_ => this);

    public bool Contains(int errorType) => _map.ContainsKey(errorType);

    public HttpStatusCode Convert(int errorType) =>
        Contains(errorType) ? _map[errorType] : HttpStatusCode.InternalServerError;

    public IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null) => 
        GetConfigurator(configure)
            .Iter(_ => _map.Clear())
            .Iter(c => c.Build()
                        .ForEach(entry => For(entry.ErrorType, entry.StatusCode)))
            .Pipe(_ => this);

    private static Identity<ErrorTypeConfigurator> GetConfigurator(Action<IErrorTypeConfigurator>? configure) =>
        new ErrorTypeConfigurator().ToIdentity()
                                   .Iter(configurator => configure?.Invoke(configurator));
}
