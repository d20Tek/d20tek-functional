namespace D20Tek.Functional.AspNetCore.WebApi;

/// <summary>
/// Extension methods for converting <see cref="Result{T}"/> into <see cref="ActionResult{T}"/>
/// responses in Web API controllers. Supports OK, Created, and CreatedAtAction patterns.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a <see cref="Result{T}"/> to an <see cref="ActionResult{T}"/>, mapping the success value to a response DTO.
    /// </summary>
    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller) where TValue : notnull =>
        result.Match(success => controller.Ok(responseMap(success)), errors => controller.Problem<TResponse>(errors));

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to an <see cref="ActionResult{T}"/>, returning a fixed response on success.
    /// </summary>
    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller) where TValue : notnull =>
        result.Match(success => controller.Ok(response), errors => controller.Problem<TResponse>(errors));

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 CreatedAtAction response, mapping the success value.
    /// </summary>
    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => controller.CreatedAtAction(routeName, routeValues, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 CreatedAtAction response with a fixed response object.
    /// </summary>
    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        result.Match(
            success => controller.CreatedAtAction(routeName, routeValues, response),
            errors => controller.Problem<TResponse>(errors));

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 Created response with the specified URI, mapping the success value.
    /// </summary>
    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        result.Match(
            success => controller.Created(routeUri, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));

    /// <summary>
    /// Converts a <see cref="Result{T}"/> to a 201 Created response with a fixed response object.
    /// </summary>
    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        result.Match(s => controller.Created(routeUri, response), errors => controller.Problem<TResponse>(errors));
}
