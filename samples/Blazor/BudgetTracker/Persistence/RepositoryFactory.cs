using Apps.Repositories;
using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.LowDb;
using D20Tek.LowDb.Browser;

namespace BudgetTracker.Persistence;

internal static class RepositoryFactory
{
    public const string _databaseKey = "wealth-data-key";

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddLocalLowDb<BudgetDataStore>(_databaseKey)
                //.AddScoped<ICategoryRepository, FileRepository<BudgetCategory, BudgetDataStore>>(sp =>
                //    new(sp.GetRequiredService<LowDb<BudgetDataStore>>(), store => store.Categories))
                //.AddScoped<IIncomeRepository, FileRepository<Income, BudgetDataStore>>(sp =>
                //    new(sp.GetRequiredService<LowDb<BudgetDataStore>>(), store => store.Incomes))
                .AddScoped<IExpenseRepository, FileRepository<Expense, BudgetDataStore>>(sp =>
                    new(sp.GetRequiredService<LowDb<BudgetDataStore>>(), store => store.Expenses))
                .AddScoped<IReconciledSnapshotRepository, FileRepository<ReconciledSnapshot, BudgetDataStore>>(sp =>
                    new(sp.GetRequiredService<LowDb<BudgetDataStore>>(), store => store.CompletedSnapshots));

        services.AddLocalLowDb<BudgetDbDocument>(_databaseKey)
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<IIncomeRepository, IncomeRepository>();

        return services;
    }
}
