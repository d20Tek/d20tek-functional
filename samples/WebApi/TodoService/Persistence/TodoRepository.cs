using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;
using TodoService.Common;
using TodoService.Endpoints.Todos;

namespace TodoService.Persistence;

internal sealed class TodoRepository : LowDbRepository<Todo, TodoDbDocument>, ITodoRepository
{
    public TodoRepository(LowDb<TodoDbDocument> db)
        : base(db, t => t.Todos)
    {
    }
}
