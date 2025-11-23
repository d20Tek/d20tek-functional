namespace D20Tek.Functional.AspNetCore;

public interface IErrorTypeConfigurator
{
    IErrorTypeConfigurator Clear();

    IErrorTypeConfigurator For(int errorType, HttpStatusCode statusCode);

    IErrorTypeConfigurator Remove(int errorType);
}