using D20Tek.Functional;
using D20Tek.Functional.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Mvc;
using TodoService.Common;

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

    private static IResult Get([FromServices]ITodoRepository repo) =>
        repo.GetAll().ToApiResult();

    private static IResult GetById([FromServices] ITodoRepository repo, int id) =>
        repo.GetById(t => t.Id, id).ToApiResult();

    private static IResult Create([FromServices] ITodoRepository repo, Todo model) =>
        model.Validate()
             .Bind(m => repo.Add(m))
             .Iter(_ => repo.SaveChanges())
             .ToCreatedApiResult($"/api/Todos/{model.Id}");

    private static IResult Update([FromServices] ITodoRepository repo, int id, Todo input) =>
        input.Validate()
             .Bind(m => repo.GetById(t => t.Id, id))
             .Map(entity => entity.Update(input.Title, input.Description, input.IsCompleted))
             .Bind(m => repo.Update(m))
             .Iter(m => repo.SaveChanges())
             .ToApiResult();

    private static IResult Delete([FromServices] ITodoRepository repo, int id) =>
        repo.GetById(t => t.Id, id)
            .Bind(entity => repo.Remove(entity))
            .Iter(m => repo.SaveChanges())
            .ToApiResult();
}
