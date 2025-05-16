using D20Tek.LowDb;
using TodoService.Common;

namespace TodoService.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseFile = "task-data.json";

    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.AddLowDb<TodoDbDocument>(_databaseFile)
                .AddSingleton<ITodoRepository, TodoRepository>();
}
