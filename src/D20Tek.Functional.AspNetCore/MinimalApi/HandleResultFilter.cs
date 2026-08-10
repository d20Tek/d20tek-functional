namespace D20Tek.Functional.AspNetCore.MinimalApi;

/// <summary>
/// An endpoint filter for Minimal APIs that automatically converts <see cref="IResultMonad"/>
/// return values into appropriate HTTP responses (200 OK or Problem Details).
/// Register with <c>.AddEndpointFilter&lt;HandleResultFilter&gt;()</c> on route handlers.
/// </summary>
public sealed class HandleResultFilter : IEndpointFilter
{
    /// <inheritdoc/>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
        (await next(context)).Pipe(
            endpointResult => endpointResult is IResultMonad result ? CovertToApiResult(result) : endpointResult);

    private static IResult CovertToApiResult(IResultMonad result) =>
        result.IsSuccess
            ? result.GetValue() is null ? TypedResults.Ok() : TypedResults.Ok(result.GetValue())
            : Results.Extensions.Problem(result.GetErrors());
}
