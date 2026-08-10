namespace D20Tek.Functional;

/// <summary>
/// Provides functional try/catch/finally wrappers that return values or <see cref="Result{T}"/>
/// instead of throwing exceptions. Enables exception handling as expressions rather than statements.
/// </summary>
public static class TryExcept
{
    /// <summary>
    /// Executes <paramref name="operation"/> and returns its result. If an exception occurs,
    /// invokes <paramref name="onException"/> to produce a fallback value.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="onException">Handler that produces a fallback value from the exception.</param>
    /// <param name="onFinally">Optional action to execute regardless of success/failure.</param>
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

    /// <summary>
    /// Executes <paramref name="operation"/>. If an exception occurs, invokes <paramref name="onException"/>.
    /// </summary>
    /// <param name="operation">The action to execute.</param>
    /// <param name="onException">Handler invoked with the exception if one occurs.</param>
    /// <param name="onFinally">Optional action to execute regardless of success/failure.</param>
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

    /// <summary>
    /// Executes <paramref name="operation"/>, passes its result to <paramref name="bind"/>, and returns the Result.
    /// If an exception occurs at any point, returns a failure Result.
    /// </summary>
    /// <typeparam name="T">The intermediate value type.</typeparam>
    /// <typeparam name="TResult">The final result value type.</typeparam>
    /// <param name="operation">A function producing the intermediate value.</param>
    /// <param name="bind">A function that transforms the intermediate value into a Result.</param>
    public static Result<TResult> Bind<T, TResult>(Func<T> operation, Func<T, Result<TResult>> bind)
        where T : notnull
        where TResult : notnull =>
        Run(() => bind(operation()), Result<TResult>.Failure);

    /// <summary>
    /// Executes <paramref name="operation"/>, applies <paramref name="mapper"/> to the result, and wraps it in a success Result.
    /// If an exception occurs, returns a failure Result.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="operation">A function producing the initial value.</param>
    /// <param name="mapper">A function to transform the value.</param>
    public static Result<T> Map<T>(Func<T> operation, Func<T, T> mapper)
        where T : notnull =>
        Run(() => Result<T>.Success(mapper(operation())), Result<T>.Failure);
}

/// <summary>
/// Provides a simplified try/catch wrapper that executes an operation returning <see cref="Result{T}"/>
/// and converts any thrown exception into a failure result.
/// </summary>
public static class Try
{
    /// <summary>
    /// Executes <paramref name="operation"/> which returns a <see cref="Result{T}"/>.
    /// If an exception is thrown, converts it to a failure Result.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="operation">A function that returns a Result.</param>
    public static Result<T> Run<T>(Func<Result<T>> operation) where T : notnull
    {
        try
        {
            return operation();
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(Error.Exception(ex));
        }
    }
}
