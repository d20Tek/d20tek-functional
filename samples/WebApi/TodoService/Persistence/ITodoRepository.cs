global using ITodoRepository = Apps.Repositories.IRepository<TodoService.Endpoints.Todos.Todo>;

using Apps.Repositories;
using TodoService.Endpoints.Todos;

namespace TodoService.Persistence;

internal sealed class TodoDataStore : DataStoreElement<Todo>
{
}
