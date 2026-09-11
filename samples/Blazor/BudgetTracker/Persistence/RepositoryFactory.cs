using BudgetTracker.Common;
using D20Tek.LowDb.Browser;

namespace BudgetTracker.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseKey = "budget-data-key";

    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddLocalLowDbAsync<BudgetDbDocument>(_databaseKey)
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<IIncomeRepository, IncomeRepository>()
                .AddScoped<IExpenseRepository, ExpenseRepository>()
                .AddScoped<IReconciledSnapshotRepository, SnapshotRepository>();
}
