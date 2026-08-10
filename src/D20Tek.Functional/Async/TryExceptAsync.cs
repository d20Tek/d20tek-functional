namespace D20Tek.Functional.Async;

/// <summary>
/// Provides async try/catch/finally wrappers that return values or <see cref="Result{T}"/>
/// instead of throwing exceptions, enabling functional error handling in async code.
/// </summary>
public static class TryExceptAsync
{
    /// <summary>
    /// Executes an async operation and returns its result. If an exception occurs,
    /// invokes <paramref name="onException"/> to produce a fallback value.
    /// </summary>
    public static async Task<T> RunAsync<T>(
        Func<Task<T>> operation, Func<Exception, T> onException, Action? onFinally = null)
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

    /// <summary>
    /// Executes an async action. If an exception occurs, invokes <paramref name="onException"/>.
    /// </summary>
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

    /// <summary>
    /// Executes an async operation, passes its result to <paramref name="bind"/>, and returns the Result.
    /// If an exception occurs, returns a failure Result.
    /// </summary>
    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        Func<Task<T>> operation, Func<T, Task<Result<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await RunAsync(async () => await bind(await operation()), Result<TResult>.Failure);

    /// <summary>
    /// Executes an async operation, applies <paramref name="mapper"/>, and wraps the result in a success Result.
    /// If an exception occurs, returns a failure Result.
    /// </summary>
    public static async Task<Result<T>> MapAsync<T>(Func<Task<T>> operation, Func<T, Task<T>> mapper)
        where T : notnull =>
        await RunAsync(async () => Result<T>.Success(await mapper(await operation())), Result<T>.Failure);
}

/// <summary>
/// Provides a simplified async try/catch wrapper that returns <see cref="Result{T}"/>,
/// converting exceptions into failure results automatically.
/// </summary>
public static class TryAsync
{
    /// <summary>
    /// Executes an async operation that returns a <see cref="Result{T}"/>.
    /// If an exception is thrown, returns a failure Result containing the exception details.
    /// </summary>
    public static async Task<Result<T>> RunAsync<T>(Func<Task<Result<T>>> operation) where T : notnull
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(Error.Exception(ex));
        }
    }
}
