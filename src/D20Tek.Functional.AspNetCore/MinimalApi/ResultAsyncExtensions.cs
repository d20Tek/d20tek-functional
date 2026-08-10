using D20Tek.Functional.Async;

namespace D20Tek.Functional.AspNetCore.MinimalApi.Async;

/// <summary>
/// Async extension methods for converting <c>Task&lt;Result&lt;T&gt;&gt;</c> into Minimal API <see cref="IResult"/> responses.
/// Supports OK, Created, and CreatedAtRoute response patterns in async pipelines.
/// </summary>
public static class ResultAsyncExtensions
{
    /// <summary>
    /// Asynchronously converts a <c>Task&lt;Result&lt;T&gt;&gt;</c> to an <see cref="IResult"/>: 200 OK on success, or Problem Details on failure.
    /// </summary>
    public static async Task<IResult> ToApiResultAsync<TValue>(this Task<Result<TValue>> result)
        where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Ok(s)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to an API response, mapping the success value to a response DTO.
    /// </summary>
    public static async Task<IResult> ToApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Ok(responseMap(s))),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to an API response, returning a fixed response object on success.
    /// </summary>
    public static async Task<IResult> ToApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Ok(response)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 CreatedAtRoute response on success.
    /// </summary>
    public static async Task<IResult> ToCreatedApiResultAsync<TValue>(
        this Task<Result<TValue>> result,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.CreatedAtRoute(s, routeName, routeValues)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 CreatedAtRoute response, mapping the success value.
    /// </summary>
    public static async Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.CreatedAtRoute(responseMap(s), routeName, routeValues)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 CreatedAtRoute response with a fixed response object.
    /// </summary>
    public static async Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.CreatedAtRoute(response, routeName, routeValues)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 Created response with the specified URI.
    /// </summary>
    public static async Task<IResult> ToCreatedApiResultAsync<TValue>(
        this Task<Result<TValue>> result, string routeUri)
        where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Created(routeUri, s)),
            e => Task.FromResult(Results.Extensions.Problem(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 Created response, mapping the success value.
    /// </summary>
    public static async Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        string routeUri) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<IResult>(TypedResults.Created(routeUri, responseMap(s))),
            e => Task.FromResult(Results.Extensions.Problem(e)));
}
