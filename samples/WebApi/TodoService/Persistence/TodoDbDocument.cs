using D20Tek.LowDb.Repositories;
using TodoService.Endpoints.Todos;

namespace TodoService.Persistence;

internal sealed class TodoDbDocument : DbDocument
{
    public HashSet<Todo> Todos { get; set; } = [];
}
