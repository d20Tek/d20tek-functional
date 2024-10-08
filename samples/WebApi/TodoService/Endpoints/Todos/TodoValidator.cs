using D20Tek.Functional;

namespace TodoService.Endpoints.Todos;

internal static class TodoValidator
{
    public static Result<Todo> Validate(this Todo todo) =>
        string.IsNullOrEmpty(todo.Title)
            ? Result<Todo>.Failure(Error.Validation("Todo.Title.Invalid", "Todo Title is required."))
            : todo;
}
