using D20Tek.Functional;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TodoService.Endpoints.Todos;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes) =>
        routes.MapGroup("/api/Todo")
              .WithTags(nameof(Todo))
              .ToIdentity()
              .Iter(g => g.MapGet("/", Get)
                          .WithName("GetAllTodos")
                          .WithOpenApi())
              .Iter(g => g.MapGet("/{id}", GetById)
                          .WithName("GetTodoById")
                          .WithOpenApi())
              .Iter(g => g.MapPut("/{id}", Update)
                          .WithName("UpdateTodo")
                          .WithOpenApi())
              .Iter(g => g.MapPost("/", Create)
                          .WithName("CreateTodo")
                          .WithOpenApi())
              .Iter(g => g.MapDelete("/{id}", Delete)
                          .WithName("DeleteTodo")
                          .WithOpenApi());

    private static Todo[] Get([FromServices]ITodoRepository repo) => repo.GetEntities();

    private static Todo GetById(int id) => new(id, "Test");

    private static Created<Todo> Create(Todo model)
    {
        model.SetId(12);
        return TypedResults.Created($"/api/Todos/{model.Id}", model);
    }

    private static NoContent Update(int id, Todo input) => TypedResults.NoContent();

    private static Ok<Todo> Delete(int id) => TypedResults.Ok(new Todo(id, "deleted"));
}
