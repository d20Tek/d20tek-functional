using D20Tek.Functional;
using D20Tek.Functional.AspNetCore.MinimalApi;
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
                          .Produces<Todo>(StatusCodes.Status200OK)
                          .ProducesProblem(StatusCodes.Status404NotFound)
                          .WithOpenApi())
              .Iter(g => g.MapPut("/{id}", Update)
                          .WithName("UpdateTodo")
                          .Produces<Todo>(StatusCodes.Status200OK)
                          .ProducesProblem(StatusCodes.Status404NotFound)
                          .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                          .WithOpenApi())
              .Iter(g => g.MapPost("/", Create)
                          .WithName("CreateTodo")
                          .Produces<Todo>(StatusCodes.Status201Created)
                          .ProducesProblem(StatusCodes.Status409Conflict)
                          .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                          .WithOpenApi())
              .Iter(g => g.MapDelete("/{id}", Delete)
                          .WithName("DeleteTodo")
                          .Produces<Todo>(StatusCodes.Status200OK)
                          .ProducesProblem(StatusCodes.Status404NotFound)
                          .WithOpenApi());

    private static Todo[] Get([FromServices]ITodoRepository repo) => repo.GetEntities();

    private static IResult GetById([FromServices] ITodoRepository repo, int id) =>
        repo.GetEntityById(id).ToApiResult();

    private static IResult Create([FromServices] ITodoRepository repo, Todo model) =>
        model.Validate()
             .Bind(m => repo.Create(m))
             .ToCreatedApiResult($"/api/Todos/{model.Id}");

    private static IResult Update([FromServices] ITodoRepository repo, int id, Todo input) =>
        input.Validate()
             .Bind(m => repo.Update(m))
             .ToApiResult();

    private static IResult Delete([FromServices] ITodoRepository repo, int id) =>
        repo.Delete(id).ToApiResult();
}
