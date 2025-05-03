namespace D20Tek.Functional;

public static class TryExcept
{
    public static T Run<T>(Func<T> operation, Func<Exception, T> onException, Action? onFinally = null)
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
        finally
        {
            onFinally?.Invoke();
        }
    }

    public static void Run(Action operation, Action<Exception> onException, Action? onFinally = null)
    {
        try
        {
            operation();
        }
        catch (Exception e)
        {
            onException(e);
        }
        finally
        {
            onFinally?.Invoke();
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
