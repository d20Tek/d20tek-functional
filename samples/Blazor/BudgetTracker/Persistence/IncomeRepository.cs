using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class IncomeRepository(LowDb<BudgetDbDocument> db) : 
    LowDbRepository<Income, BudgetDbDocument>(db, i => i.Incomes.Entities), IIncomeRepository
{
    public Result<IEnumerable<Income>> RemoveByDateRange(DateRange range) =>
        Find(i => range.InRange(i.DepositDate))
            .Bind(i => RemoveRange(i));
}
