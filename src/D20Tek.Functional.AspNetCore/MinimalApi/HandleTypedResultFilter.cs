namespace D20Tek.Functional.AspNetCore.MinimalApi;

public sealed class HandleTypedResultFilter<T> : IEndpointFilter
    where T : class
{
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
