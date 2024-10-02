namespace D20Tek.Functional;

public static class TryExcept
{
    public static T Run<T>(Func<T> operation, Func<Exception, T> onException)
        where T : notnull
    {
        try
        {
            return operation();
        }
        catch (Exception e)
        {
            return onException(e);
        }
    }

    public static Result<TResult> Bind<T, TResult>(Func<T> operation, Func<T, Result<TResult>> bind)
        where T : notnull
        where TResult : notnull =>
        Run(
            () => bind(operation()),
            ex => Result<TResult>.Failure(ex));

    public static Result<T> Map<T>(Func<T> operation, Func<T, T> mapper)
        where T : notnull =>
        Run(
            () => Result<T>.Success(mapper(operation())),
            ex => Result<T>.Failure(ex));
}
