using D20Tek.Functional.Async;

namespace D20Tek.Functional.AspNetCore.WebApi.Async;

public static class ResultAsyncExtensions
{
    public static async Task<ActionResult<TResponse>> ToActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Ok(responseMap(s))),
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    public static async Task<ActionResult<TResponse>> ToActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        ControllerBase controller) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Ok(response)), 
            e => Task.FromResult(controller.Problem<TResponse>(e)));

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

    public static async Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Created(routeUri, responseMap(s))),
            e => Task.FromResult(controller.Problem<TResponse>(e)));

    public static async Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(
        this Task<Result<TValue>> result,
        TResponse response,
        ControllerBase controller,
        string routeUri) where TValue : notnull =>
        await result.MatchAsync(
            s => Task.FromResult<ActionResult<TResponse>>(controller.Created(routeUri, response)),
            e => Task.FromResult(controller.Problem<TResponse>(e)));
}
