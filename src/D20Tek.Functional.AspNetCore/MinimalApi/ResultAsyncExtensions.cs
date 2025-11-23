using D20Tek.Functional.Async;

namespace D20Tek.Functional.AspNetCore.MinimalApi.Async;

public static class ResultAsyncExtensions
{
    public static async Task<IResult> ToApiResultAsync<TValue>(this Task<Result<TValue>> result)
        where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Ok(s)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Ok(responseMap(s))),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Ok(response)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToCreatedApiResultAsync<TValue>(
        this Task<Result<TValue>> result,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.CreatedAtRoute(s, routeName, routeValues)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.CreatedAtRoute(responseMap(s), routeName, routeValues)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.CreatedAtRoute(response, routeName, routeValues)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToCreatedApiResultAsync<TValue>(
        this Task<Result<TValue>> result, string routeUri)
        where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Created(routeUri, s)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    public static async Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        string routeUri) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Created(routeUri, responseMap(s))),
            e => Task.FromResult(Results.Extensions.Problem(e)));
}
