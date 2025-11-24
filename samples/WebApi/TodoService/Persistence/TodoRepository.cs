using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;
using TodoService.Common;
using TodoService.Endpoints.Todos;

namespace TodoService.Persistence;

internal sealed class TodoRepository(LowDb<TodoDbDocument> db) : 
    LowDbRepository<Todo, TodoDbDocument>(db, t => t.Todos), ITodoRepository
{
}
