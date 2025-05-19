using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Common;

internal interface ICategoryRepository : IRepository<BudgetCategory>;

internal interface IExpenseRepository : IRepository<Expense>
{
    Result<IEnumerable<Expense>> GetExpensesToReconcile(int catId, DateRange range);

    Result<IEnumerable<Expense>> RemoveByDateRange(DateRange range);
}

internal interface IIncomeRepository : IRepository<Income>
{
    Result<IEnumerable<Income>> RemoveByDateRange(DateRange range);
}

internal interface IReconciledSnapshotRepository : IRepository<ReconciledSnapshot>
{
    Result<ReconciledSnapshot> GetSnapshotForMonth(DateTimeOffset date);

    Result<IEnumerable<ReconciledSnapshot>> GetSnapshotsForDateRange(DateRange range);
}
