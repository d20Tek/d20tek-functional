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
