using D20Tek.LowDb;

namespace BudgetTracker.Persistence;

internal static class RepositoryFactory
{
    private static readonly LowDb<BudgetDataStore> _dataStore =
        LowDbFactory.CreateLowDb<BudgetDataStore>(b =>
            b.UseFileDatabase(Constants.DatabaseFile)
                .WithFolder(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData) + Constants.AppFolder));

    public static ICategoryRepository CreateCategoryRepository() =>
        new CategoryFileRepository(_dataStore);
}
