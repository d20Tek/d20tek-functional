using Apps.Repositories;
using D20Tek.LowDb;

namespace WealthTracker.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseFile = "wealth-data-test.json";
    public const string _appFolder = "\\d20tek-fin";

    private static readonly LowDb<WealthDataStore> _dataStore =
        LowDbFactory.CreateLowDb<WealthDataStore>(b =>
            b.UseFileDatabase(_databaseFile)
                .WithFolder(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData) + _appFolder));

    public static IWealthRepository CreateWealthRepository() =>
        new FileRepository<WealthDataEntry, WealthDataStore>(_dataStore, store => store);
}
