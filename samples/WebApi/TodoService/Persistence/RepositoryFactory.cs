using Apps.Repositories;
using D20Tek.LowDb;
using TodoService.Endpoints.Todos;

namespace TodoService.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseFile = "task-data.json";

    public static IServiceCollection AddRepository(this IServiceCollection services) => 
        services.AddLowDb<TodoDataStore>(_databaseFile)
                .AddSingleton<ITodoRepository, FileRepository<Todo, TodoDataStore>>(sp =>
                    new(sp.GetRequiredService<LowDb<TodoDataStore>>(), store => store));
}
