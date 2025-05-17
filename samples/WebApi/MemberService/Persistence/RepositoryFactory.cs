using D20Tek.LowDb;
using MemberService.Common;

namespace MemberService.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseFile = "member-data.json";

    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.AddLowDb<MemberDbDocument>(_databaseFile)
                .AddSingleton<IMemberRepository, MemberRepository>();
}
