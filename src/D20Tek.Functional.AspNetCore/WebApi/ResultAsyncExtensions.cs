using D20Tek.Functional.Async;

namespace D20Tek.Functional.AspNetCore.WebApi.Async;

/// <summary>
/// Async extension methods for converting <c>Task&lt;Result&lt;T&gt;&gt;</c> into <see cref="ActionResult{T}"/>
/// responses in Web API controllers. Supports OK, Created, and CreatedAtAction patterns in async pipelines.
/// </summary>
public static class ResultAsyncExtensions
{
    /// <summary>
    /// Asynchronously converts a result to an <see cref="ActionResult{T}"/>, mapping the success value to a response DTO.
    /// </summary>
    public static async Task<ActionResult<TResponse>> ToActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Ok(responseMap(s))),
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    /// <summary>
    /// Asynchronously converts a result to an <see cref="ActionResult{T}"/>, returning a fixed response on success.
    /// </summary>
    public static async Task<ActionResult<TResponse>> ToActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        ControllerBase controller) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Ok(response)), 
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 CreatedAtAction response, mapping the success value.
    /// </summary>
    public static async Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(
                        controller.CreatedAtAction(routeName, routeValues, responseMap(s))),
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 CreatedAtAction response with a fixed response object.
    /// </summary>
    public static async Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(
                        controller.CreatedAtAction(routeName, routeValues, response)),
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 Created response with the specified URI, mapping the success value.
    /// </summary>
    public static async Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Created(routeUri, responseMap(s))),
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    /// <summary>
    /// Asynchronously converts a result to a 201 Created response with a fixed response object.
    /// </summary>
    public static async Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Created(routeUri, response)),
            e => Task.FromResult(controller.Problem<TResponse>(e)));
}
