using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class ExpenseRepository(LowDbAsync<BudgetDbDocument> db) : 
    LowDbAsyncRepository<Expense, BudgetDbDocument>(db, e => e.Expenses.Entities), IExpenseRepository
{
    public Task<Result<IEnumerable<Expense>>> GetExpensesToReconcile(int catId, DateRange range) =>
        GetAllAsync().MapAsync(e => Task.FromResult(e.Where(x => x.CategoryId == catId)
                     .Where(x => range.InRange(x.CommittedDate))));

    public Task<Result<IEnumerable<Expense>>> RemoveByDateRange(DateRange range) =>
        FindAsync(x => range.InRange(x.CommittedDate))
            .BindAsync(e => RemoveRangeAsync(e));
}
