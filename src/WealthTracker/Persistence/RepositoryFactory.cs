using D20Tek.LowDb;

namespace WealthTracker.Persistence;

internal static class RepositoryFactory
{
    public static IWealthRepository CreateWealthRepository() =>
        new WealthFileRepository(
            LowDbFactory.CreateLowDb<WealthDataStore>(b => 
                b.UseFileDatabase(Constants.DatabaseFile)
                    .WithFolder(Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData) + Constants.AppFolder))
                );
}
