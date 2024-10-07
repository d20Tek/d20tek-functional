namespace TodoService.Endpoints.Todos;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Todo")
                          .WithTags(nameof(Todo));

        group.MapGet("/", () =>
        {
            return new [] { new Todo() };
        })
        .WithName("GetAllTodos")
        .WithOpenApi();

        group.MapGet("/{id}", (int id) =>
        {
            //return new Todo { ID = id };
        })
        .WithName("GetTodoById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Todo input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateTodo")
        .WithOpenApi();

        group.MapPost("/", (Todo model) =>
        {
            //return TypedResults.Created($"/api/Todos/{model.ID}", model);
        })
        .WithName("CreateTodo")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Todo { ID = id });
        })
        .WithName("DeleteTodo")
        .WithOpenApi();
    }
}
