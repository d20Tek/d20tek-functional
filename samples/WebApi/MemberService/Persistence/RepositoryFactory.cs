using Apps.Repositories;
using D20Tek.LowDb;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseFile = "member-data.json";

    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.AddLowDb<TodoDataStore>(_databaseFile)
                .AddSingleton<IMemberRepository, FileRepository<MemberEntity, TodoDataStore>>(sp =>
                    new(sp.GetRequiredService<LowDb<TodoDataStore>>(), store => store));
}
