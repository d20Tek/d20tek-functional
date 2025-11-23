namespace D20Tek.Functional.AspNetCore.MinimalApi;

public sealed class HandleResultFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
        (await next(context)).Pipe(
            endpointResult => endpointResult is IResultMonad result ? CovertToApiResult(result) : endpointResult);

    private static IResult CovertToApiResult(IResultMonad result) =>
        result.IsSuccess
            ? result.GetValue() is null ? TypedResults.Ok() : TypedResults.Ok(result.GetValue())
            : Results.Extensions.Problem(result.GetErrors());
}
