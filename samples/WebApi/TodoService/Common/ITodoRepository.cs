using D20Tek.LowDb.Repositories;
using TodoService.Endpoints.Todos;

namespace TodoService.Common;

internal interface ITodoRepository : IRepository<Todo>;
