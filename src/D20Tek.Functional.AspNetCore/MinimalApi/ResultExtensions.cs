namespace D20Tek.Functional.AspNetCore.MinimalApi;

/// <summary>
/// Extension methods for converting <see cref="Result{T}"/> into Minimal API <see cref="IResult"/> responses.
/// Supports OK, Created, and CreatedAtRoute response patterns.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a <see cref="Result{T}"/> to an <see cref="IResult"/>: 200 OK on success, or Problem Details on failure.
    /// </summary>
    public static IResult ToApiResult<TValue>(this Result<TValue> result) where TValue : notnull =>
        result.Match(success => TypedResults.Ok(success), Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to an <see cref="IResult"/>, mapping the success value to a response DTO.
    /// </summary>
    public static IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap) where TValue : notnull =>
        result.Match(s => TypedResults.Ok(responseMap(s)), Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to an <see cref="IResult"/>, returning a fixed response object on success.
    /// </summary>
    public static IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response) where TValue : notnull =>
        result.Match(success => TypedResults.Ok(response), Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 CreatedAtRoute response on success, or Problem Details on failure.
    /// </summary>
    public static IResult ToCreatedApiResult<TValue>(
        this Result<TValue> result,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => TypedResults.CreatedAtRoute(success, routeName, routeValues),
            Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 CreatedAtRoute response, mapping the success value to a response DTO.
    /// </summary>
    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => TypedResults.CreatedAtRoute(responseMap(success), routeName, routeValues),
            Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 CreatedAtRoute response with a fixed response object.
    /// </summary>
    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => TypedResults.CreatedAtRoute(response, routeName, routeValues),
            Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 Created response with the specified URI.
    /// </summary>
    public static IResult ToCreatedApiResult<TValue>(this Result<TValue> result, string routeUri)
        where TValue : notnull =>
        result.Match(success => TypedResults.Created(routeUri, success), Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 Created response, mapping the success value to a response DTO.
    /// </summary>
    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string routeUri) where TValue : notnull =>
        result.Match(s => TypedResults.Created(routeUri, responseMap(s)), Results.Extensions.Problem);

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 Created response with a fixed response object.
    /// </summary>
    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string routeUri) where TValue : notnull =>
        result.Match(s => TypedResults.Created(routeUri, response), Results.Extensions.Problem);
}
