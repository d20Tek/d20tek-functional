using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Functional.AspNetCore.WebApi;

public static class ResultExtensions
{
    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller) where TValue : notnull =>
        result.Match(success => controller.Ok(responseMap(success)), errors => controller.Problem<TResponse>(errors));

    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller) where TValue : notnull =>
        result.Match(success => controller.Ok(response), errors => controller.Problem<TResponse>(errors));

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => controller.CreatedAtAction(routeName, routeValues, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => controller.CreatedAtAction(routeName, routeValues, response),
            errors => controller.Problem<TResponse>(errors));

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        result.Match(
            success => controller.Created(routeUri, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        result.Match(s => controller.Created(routeUri, response), errors => controller.Problem<TResponse>(errors));
}
