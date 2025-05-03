namespace D20Tek.Functional.Async;

public static class TryExceptAsync
{
    public static async Task<T> RunAsync<T>(
        Func<Task<T>> operation,
        Func<Exception, T> onException,
        Action? onFinally = null)
        where T : notnull
    {
        try
        {
            return await operation();
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

    public static async Task RunAsync(Func<Task> operation, Action<Exception> onException, Action? onFinally = null)
    {
        try
        {
            await operation();
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

    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        Func<Task<T>> operation,
        Func<T, Task<Result<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await RunAsync(
            async () => await bind(await operation()),
            ex => Result<TResult>.Failure(ex));

    public static async Task<Result<T>> MapAsync<T>(Func<Task<T>> operation, Func<T, Task<T>> mapper)
        where T : notnull =>
        await RunAsync(
            async () => Result<T>.Success(await mapper(await operation())),
            ex => Result<T>.Failure(ex));
}
