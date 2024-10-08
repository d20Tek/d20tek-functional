using D20Tek.Functional;
using Microsoft.AspNetCore.Mvc;

namespace TodoService.Endpoints.Todos;

internal static class TodoEndpoints
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

    private static IResult GetById([FromServices] ITodoRepository repo, int id) =>
        repo.GetEntityById(id)
            .ToTypedResult("Error occurred getting Todo by id.");

    private static IResult Create([FromServices] ITodoRepository repo, Todo model) =>
        model.Validate()
             .Bind(m => repo.Create(m))
             .ToTypedResult("Error occurred saving Todo.");

    private static IResult Update([FromServices] ITodoRepository repo, int id, Todo input) =>
        input.Validate()
             .Bind(m => repo.Update(m))
             .ToTypedResult("Error occurred updating Todo.");

    private static IResult Delete([FromServices] ITodoRepository repo, int id) =>
        repo.Delete(id)
            .ToTypedResult("Error occurred deleting Todo.");
}
