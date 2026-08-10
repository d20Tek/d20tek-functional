namespace D20Tek.Functional.AspNetCore.MinimalApi;

/// <summary>
/// A typed endpoint filter for Minimal APIs that converts <see cref="Result{T}"/>
/// return values into appropriate HTTP responses. Use when the endpoint's return type is known at compile time.
/// Register with <c>.AddEndpointFilter&lt;HandleTypedResultFilter&lt;T&gt;&gt;()</c>.
/// </summary>
/// <typeparam name="T">The expected success value type of the Result.</typeparam>
public sealed class HandleTypedResultFilter<T> : IEndpointFilter
    where T : class
{
    /// <inheritdoc/>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpointResult = await next(context);
        if (endpointResult is Result<T> result)
        {
            return result.IsSuccess ? TypedResults.Ok(result.GetValue())
                                    : Results.Extensions.Problem(result.GetErrors());
        }

        return endpointResult;
    }
}
