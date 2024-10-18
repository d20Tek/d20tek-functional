using Apps.Repositories;
using D20Tek.LowDb;
using WealthTracker.Domain;

namespace WealthTracker.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseFile = "wealth-data.json";
    public const string _appFolder = "\\d20tek-fin";

    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.AddLowDb<WealthDataStore>(b =>
                    b.UseFileDatabase(_databaseFile)
                     .WithFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _appFolder))
                .AddSingleton<IWealthRepository, FileRepository<WealthDataEntity, WealthDataStore>>(sp =>
                    new(sp.GetRequiredService<LowDb<WealthDataStore>>(), store => store));
}
