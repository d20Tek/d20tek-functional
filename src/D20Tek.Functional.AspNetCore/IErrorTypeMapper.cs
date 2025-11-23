namespace D20Tek.Functional.AspNetCore;

public interface IErrorTypeMapper
{
    IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null);

    bool Contains(int errorType);
        
    HttpStatusCode Convert(int errorType);

    IErrorTypeMapper For(int errorType, HttpStatusCode statusCode);

    IErrorTypeMapper Remove(int errorType);
}
