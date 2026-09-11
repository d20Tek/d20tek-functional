using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Common;

internal interface ICategoryRepository : IRepositoryAsync<BudgetCategory>;

internal interface IExpenseRepository : IRepositoryAsync<Expense>
{
    Task<Result<IEnumerable<Expense>>> GetExpensesToReconcile(int catId, DateRange range);

    Task<Result<IEnumerable<Expense>>> RemoveByDateRange(DateRange range);
}

internal interface IIncomeRepository : IRepositoryAsync<Income>
{
    Task<Result<IEnumerable<Income>>> RemoveByDateRange(DateRange range);
}

internal interface IReconciledSnapshotRepository : IRepositoryAsync<ReconciledSnapshot>
{
    Task<Result<ReconciledSnapshot>> GetSnapshotForMonth(DateTimeOffset date);

    Task<Result<IEnumerable<ReconciledSnapshot>>> GetSnapshotsForDateRange(DateRange range);
}
