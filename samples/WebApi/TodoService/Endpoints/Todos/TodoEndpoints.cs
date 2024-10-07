using D20Tek.Functional;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TodoService.Endpoints.Todos;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes) =>
        routes.MapGroup("/api/Todo")
              .WithTags(nameof(Todo))
              .ToIdentity()
              .Iter(g => g.MapGet("/", () => Get())
                          .WithName("GetAllTodos")
                          .WithOpenApi())
              .Iter(g => g.MapGet("/{id}", (int id) => GetById(id))
                          .WithName("GetTodoById")
                          .WithOpenApi())
              .Iter(g => g.MapPut("/{id}", (int id, Todo input) => Update(id, input))
                          .WithName("UpdateTodo")
                          .WithOpenApi())
              .Iter(g => g.MapPost("/", (Todo model) => Create(model))
                          .WithName("CreateTodo")
                          .WithOpenApi())
              .Iter(g => g.MapDelete("/{id}", (int id) => Delete(id))
                          .WithName("DeleteTodo")
                          .WithOpenApi());

    private static Todo[] Get() => [new(1, "Test")];

    private static Todo GetById(int id) => new(id, "Test");

    private static Created<Todo> Create(Todo model)
    {
        model.SetId(12);
        return TypedResults.Created($"/api/Todos/{model.Id}", model);
    }

    private static NoContent Update(int id, Todo input) => TypedResults.NoContent();

    private static Ok<Todo> Delete(int id) => TypedResults.Ok(new Todo(id, "deleted"));
}
