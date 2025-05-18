using D20Tek.LowDb.Browser;
using WealthTracker.Common;

namespace WealthTracker.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseKey = "wealth-data-key";

    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services.AddLocalLowDb<WealthDbDocument>(_databaseKey)
                .AddScoped<IWealthRepository, WealthRepository>();
}
