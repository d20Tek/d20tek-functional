using D20Tek.Functional;
using TodoService.Endpoints.Todos;

namespace TodoService.Endpoints;

internal static class ResultExtensions
{
    public static IResult ToTypedResult<T>(this Result<T> result, string message) where T : notnull =>
        result switch
        {
            Success<Todo> s => TypedResults.Ok(s.GetValue()),
            _ => TypedResults.Problem(message, null, StatusCodes.Status400BadRequest)
        };
}
