using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class IncomeRepository(LowDbAsync<BudgetDbDocument> db) : 
    LowDbAsyncRepository<Income, BudgetDbDocument>(db, i => i.Incomes.Entities), IIncomeRepository
{
    public Task<Result<IEnumerable<Income>>> RemoveByDateRange(DateRange range) =>
        FindAsync(i => range.InRange(i.DepositDate))
            .BindAsync(i => RemoveRangeAsync(i));
}
