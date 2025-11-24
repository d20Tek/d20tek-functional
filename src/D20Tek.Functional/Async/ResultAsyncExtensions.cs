namespace D20Tek.Functional.Async;

public static class ResultAsyncExtensions
{
    public static async Task<TResult> MatchAsync<TIn, TResult>(
        this Result<TIn> result, Func<TIn, Task<TResult>> onSuccess, Func<Error[], Task<TResult>> onFailure)
        where TIn : notnull
        where TResult : notnull =>
        result.IsSuccess ? await onSuccess(result.GetValue()) : await onFailure(result.GetErrors());

    public static async Task<TResult> MatchAsync<TIn, TResult>(
        this Task<Result<TIn>> result, Func<TIn, Task<TResult>> onSuccess, Func<Error[], Task<TResult>> onFailure)
        where TIn : notnull
        where TResult : notnull =>
        await (await result).MatchAsync(onSuccess, onFailure);

    public static async Task<Result<TResult>> BindAsync<TIn, TResult>(
        this Result<TIn> result, Func<TIn, Task<Result<TResult>>> binder)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(binder, e => Task.FromResult(Result<TResult>.Failure(e)));

    public static async Task<Result<TResult>> BindAsync<TIn, TResult>(
        this Task<Result<TIn>> result, Func<TIn, Task<Result<TResult>>> binder)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(binder, e => Task.FromResult(Result<TResult>.Failure(e)));

    public static async Task<T> DefaultWithAsync<T>(this Result<T> result, Func<Task<T>> func)
        where T : notnull =>
        await result.MatchAsync(v => Task.FromResult(v), async (_) => await func());

    public static async Task<T> DefaultWithAsync<T>(this Task<Result<T>> result, Func<Task<T>> func)
        where T : notnull =>
        await result.MatchAsync(v => Task.FromResult(v), async (_) => await func());

    public static async Task<bool> ExistsAsync<T>(this Result<T> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), e => Task.FromResult(false));

    public static async Task<bool> ExistsAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), _ => Task.FromResult(false));

    public static async Task<Result<T>> FilterAsync<T>(this Result<T> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(
            async (v) => await predicate(v) ? Result<T>.Success(v) : Result<T>.Failure(Constants.ResultFilterError),
            e => Task.FromResult(Result<T>.Failure(e)));

    public static async Task<Result<T>> FilterAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(
            async (v) => await predicate(v) ? Result<T>.Success(v) : Result<T>.Failure(Constants.ResultFilterError),
            e => Task.FromResult(Result<T>.Failure(e)));

    public static async Task<TResult> FoldAsync<T, TResult>(
        this Result<T> result, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(initial, v), _ => Task.FromResult(initial));

    public static async Task<TResult> FoldAsync<T, TResult>(
        this Task<Result<T>> result, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(initial, v), _ => Task.FromResult(initial));

    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Result<T> result, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(v, initial), _ => Task.FromResult(initial));

    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Task<Result<T>> result, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await result.MatchAsync(async (v) => await func(v, initial), _ => Task.FromResult(initial));

    public static async Task<bool> ForAllAsync<T>(this Result<T> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), _ => Task.FromResult(true));

    public static async Task<bool> ForAllAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await result.MatchAsync(async (v) => await predicate(v), _ => Task.FromResult(true));

    public static async Task<Result<T>> IterAsync<T>(this Result<T> result, Func<T, Task> action)
        where T : notnull
    {
        if (result.IsSuccess) await action(result.GetValue());
        return result;
    }

    public static async Task<Result<T>> IterAsync<T>(this Task<Result<T>> result, Func<T, Task> action)
        where T : notnull =>
        await (await result).IterAsync(action);

    public static async Task<Result<TResult>> MapAsync<TIn, TResult>(
        this Result<TIn> result, Func<TIn, Task<TResult>> mapper)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(
            async (v) => Result<TResult>.Success(await mapper(v)),
            e => Task.FromResult(Result<TResult>.Failure(e)));

    public static async Task<Result<TResult>> MapAsync<TIn, TResult>(
        this Task<Result<TIn>> result, Func<TIn, Task<TResult>> mapper)
        where TIn : notnull
        where TResult : notnull =>
        await result.MatchAsync(
            async (v) => Result<TResult>.Success(await mapper(v)),
            e => Task.FromResult(Result<TResult>.Failure(e)));

    public static async Task<Result<TResult>> MapErrorsAsync<TIn, TResult>(this Task<Result<TIn>> result)
        where TIn : notnull
        where TResult : notnull =>
        (await result).MapErrors<TResult>();
}
