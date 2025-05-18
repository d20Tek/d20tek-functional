using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class IncomeRepository : LowDbRepository<Income, BudgetDbDocument>, IIncomeRepository
{
    public IncomeRepository(LowDb<BudgetDbDocument> db)
        : base(db, i => i.Incomes.Entities)
    {
    }

    public Result<IEnumerable<Income>> RemoveByDateRange(DateRange range) =>
        Find(i => range.InRange(i.DepositDate))
            .Bind(i => RemoveRange(i));
}
