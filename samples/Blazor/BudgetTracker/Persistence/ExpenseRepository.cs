using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class ExpenseRepository : LowDbRepository<Expense, BudgetDbDocument>, IExpenseRepository
{
    public ExpenseRepository(LowDb<BudgetDbDocument> db)
        : base(db, e => e.Expenses.Entities)
    {
    }

    public Result<IEnumerable<Expense>> GetExpensesToReconcile(int catId, DateRange range) =>
        GetAll().Map(e => e.Where(x => x.CategoryId == catId)
                           .Where(x => range.InRange(x.CommittedDate)));

    public Result<IEnumerable<Expense>> RemoveByDateRange(DateRange range) =>
        Find(x => range.InRange(x.CommittedDate))
            .Bind(e => RemoveRange(e));
}
