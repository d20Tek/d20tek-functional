using Apps.Repositories;
using D20Tek.LowDb;
using D20Tek.LowDb.Browser;
using WealthTracker.Domain;

namespace WealthTracker.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseKey = "wealth-data-key";

    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.AddLocalLowDb<WealthDataStore>(_databaseKey)
                .AddScoped<IWealthRepository, FileRepository<WealthDataEntity, WealthDataStore>>(sp =>
                    new(sp.GetRequiredService<LowDb<WealthDataStore>>(), store => store));
}
