using Microsoft.AspNetCore.Http;

namespace D20Tek.Functional.AspNetCore.MinimalApi;

public static class ResultExtensions
{
    public static IResult ToApiResult<TValue>(this Result<TValue> result) where TValue : notnull =>
        result.Match(success => TypedResults.Ok(success), errors => Results.Extensions.Problem(errors));

    public static IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap) where TValue : notnull =>
        result.Match(s => TypedResults.Ok(responseMap(s)), errors => Results.Extensions.Problem(errors));

    public static IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response) where TValue : notnull =>
        result.Match(success => TypedResults.Ok(response), errors => Results.Extensions.Problem(errors));

    public static IResult ToCreatedApiResult<TValue>(
        this Result<TValue> result,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => TypedResults.CreatedAtRoute(success, routeName, routeValues),
            errors => Results.Extensions.Problem(errors));

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => TypedResults.CreatedAtRoute(responseMap(success), routeName, routeValues),
            errors => Results.Extensions.Problem(errors));

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => TypedResults.CreatedAtRoute(response, routeName, routeValues),
            errors => Results.Extensions.Problem(errors));

    public static IResult ToCreatedApiResult<TValue>(this Result<TValue> result, string routeUri)
        where TValue : notnull =>
        result.Match(success => TypedResults.Created(routeUri, success), errors => Results.Extensions.Problem(errors));

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string routeUri) where TValue : notnull =>
        result.Match(s => TypedResults.Created(routeUri, responseMap(s)), errors => Results.Extensions.Problem(errors));

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string routeUri) where TValue : notnull =>
        result.Match(s => TypedResults.Created(routeUri, response), errors => Results.Extensions.Problem(errors));
}
