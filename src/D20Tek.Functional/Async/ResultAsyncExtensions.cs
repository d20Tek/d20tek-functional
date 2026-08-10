namespace D20Tek.Functional.Async;

/// <summary>
/// Async extension methods for <see cref="Result{T}"/> providing asynchronous Match, Bind, Map, Filter,
/// Fold, Iter, and other monadic operations. Each method has overloads for both <c>Result&lt;T&gt;</c>
/// and <c>Task&lt;Result&lt;T&gt;&gt;</c> to enable seamless async pipeline chaining.
/// </summary>
public static class ResultAsyncExtensions
{
    /// <summary>
    /// Asynchronously pattern-matches a <see cref="Result{T}"/>, invoking <paramref name="onSuccess"/> or <paramref name="onFailure"/>.
    /// </summary>
    public static async Task<TResult> MatchAsync<TIn, TResult>(
        this Result<TIn> result, Func<TIn, Task<TResult>> onSuccess, Func<Error[], Task<TResult>> onFailure)
        where TIn : notnull
        where TResult : notnull =>
        result.IsSuccess ? await onSuccess(result.GetValue()) : await onFailure(result.GetErrors());

    /// <summary>
    /// Awaits the result, then asynchronously pattern-matches it via <paramref name="onSuccess"/> or <paramref name="onFailure"/>.
    /// </summary>
    public static async Task<TResult> MatchAsync<TIn, TResult>(
        this Task<Result<TIn>> result, Func<TIn, Task<TResult>> onSuccess, Func<Error[], Task<TResult>> onFailure)
        where TIn : notnull
        where TResult : notnull =>
        await (await result).MatchAsync(onSuccess, onFailure);

    /// <summary>
    /// Asynchronously binds the success value of a <see cref="Result{T}"/> to produce a new Result.
    /// </summary>
    public static async Task<Result<TResult>> BindAsync<TIn, TResult>(
        this Result<TIn> result, Func<TIn, Task<Result<TResult>>> binder)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(binder, e => Task.FromResult(Result<TResult>.Failure(e)));

    /// <summary>
    /// Awaits the result, then asynchronously binds the success value to produce a new Result.
    /// </summary>
    public static async Task<Result<TResult>> BindAsync<TIn, TResult>(
        this Task<Result<TIn>> result, Func<TIn, Task<Result<TResult>>> binder)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(binder, e => Task.FromResult(Result<TResult>.Failure(e)));

    /// <summary>
    /// Returns the success value if present; otherwise asynchronously produces a default via <paramref name="func"/>.
    /// </summary>
    public static async Task<T> DefaultWithAsync<T>(this Result<T> result, Func<Task<T>> func)
        where T : notnull =>
        await result.MatchAsync(v => Task.FromResult(v), async (_) => await func());

    /// <summary>
    /// Awaits the result, then returns the success value or asynchronously produces a default.
    /// </summary>
    public static async Task<T> DefaultWithAsync<T>(this Task<Result<T>> result, Func<Task<T>> func)
        where T : notnull =>
        await result.MatchAsync(v => Task.FromResult(v), async (_) => await func());

    /// <summary>
    /// Asynchronously tests whether the success value satisfies <paramref name="predicate"/>. Returns false on failure.
    /// </summary>
    public static async Task<bool> ExistsAsync<T>(this Result<T> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), e => Task.FromResult(false));

    /// <summary>
    /// Awaits the result, then asynchronously tests whether the success value satisfies <paramref name="predicate"/>.
    /// </summary>
    public static async Task<bool> ExistsAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), _ => Task.FromResult(false));

    /// <summary>
    /// Asynchronously filters the result, returning a failure if the predicate is not satisfied.
    /// </summary>
    public static async Task<Result<T>> FilterAsync<T>(this Result<T> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(
            async (v) => await predicate(v) ? Result<T>.Success(v) : Result<T>.Failure(Constants.ResultFilterError),
            e => Task.FromResult(Result<T>.Failure(e)));

    /// <summary>
    /// Awaits the result, then asynchronously filters it, returning a failure if the predicate is not satisfied.
    /// </summary>
    public static async Task<Result<T>> FilterAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(
            async (v) => await predicate(v) ? Result<T>.Success(v) : Result<T>.Failure(Constants.ResultFilterError),
            e => Task.FromResult(Result<T>.Failure(e)));

    /// <summary>
    /// Asynchronously folds the success value with an initial state. Returns <paramref name="initial"/> on failure.
    /// </summary>
    public static async Task<TResult> FoldAsync<T, TResult>(
        this Result<T> result, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(initial, v), _ => Task.FromResult(initial));

    /// <summary>
    /// Awaits the result, then asynchronously folds the success value with an initial state.
    /// </summary>
    public static async Task<TResult> FoldAsync<T, TResult>(
        this Task<Result<T>> result, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(initial, v), _ => Task.FromResult(initial));

    /// <summary>
    /// Asynchronously folds the success value with reversed parameter order. Returns <paramref name="initial"/> on failure.
    /// </summary>
    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Result<T> result, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(v, initial), _ => Task.FromResult(initial));

    /// <summary>
    /// Awaits the result, then asynchronously folds the success value with reversed parameter order.
    /// </summary>
    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Task<Result<T>> result, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(v, initial), _ => Task.FromResult(initial));

    /// <summary>
    /// Returns true if the result is a failure or the success value satisfies <paramref name="predicate"/> asynchronously.
    /// </summary>
    public static async Task<bool> ForAllAsync<T>(this Result<T> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), _ => Task.FromResult(true));

    /// <summary>
    /// Awaits the result, then returns true if failure or the success value satisfies <paramref name="predicate"/>.
    /// </summary>
    public static async Task<bool> ForAllAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), _ => Task.FromResult(true));

    /// <summary>
    /// Asynchronously executes <paramref name="action"/> on the success value, then returns the original result.
    /// </summary>
    public static async Task<Result<T>> IterAsync<T>(this Result<T> result, Func<T, Task> action)
        where T : notnull
    {
        if (result.IsSuccess) await action(result.GetValue());
        return result;
    }

    /// <summary>
    /// Awaits the result, then asynchronously executes <paramref name="action"/> on the success value.
    /// </summary>
    public static async Task<Result<T>> IterAsync<T>(this Task<Result<T>> result, Func<T, Task> action)
        where T : notnull =>
        await (await result).IterAsync(action);

    /// <summary>
    /// Asynchronously transforms the success value using <paramref name="mapper"/>, propagating errors on failure.
    /// </summary>
    public static async Task<Result<TResult>> MapAsync<TIn, TResult>(
        this Result<TIn> result, Func<TIn, Task<TResult>> mapper)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(
            async (v) => Result<TResult>.Success(await mapper(v)),
            e => Task.FromResult(Result<TResult>.Failure(e)));

    /// <summary>
    /// Awaits the result, then asynchronously transforms the success value using <paramref name="mapper"/>.
    /// </summary>
    public static async Task<Result<TResult>> MapAsync<TIn, TResult>(
        this Task<Result<TIn>> result, Func<TIn, Task<TResult>> mapper)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(
            async (v) => Result<TResult>.Success(await mapper(v)),
            e => Task.FromResult(Result<TResult>.Failure(e)));

    /// <summary>
    /// Awaits the result and maps its errors to a new result type, discarding the original success value.
    /// </summary>
    public static async Task<Result<TResult>> MapErrorsAsync<TIn, TResult>(this Task<Result<TIn>> result)
        where TIn : notnull
        where TResult : notnull =>
        (await result).MapErrors<TResult>();
}
