using BudgetTracker.Domain;
using D20Tek.Functional;

namespace BudgetTracker.Persistence;

internal static class ExpenseRepositoryExtensions
{
    public static Expense[] GetExpensesToReconcile(this IExpenseRepository expRepo, int catId, DateRange range) =>
        expRepo.GetEntities().Where(x => x.CategoryId == catId)
                             .Where(x => range.InRange(x.CommittedDate))
                             .ToArray();

    public static Result<Expense[]> DeleteByDateRange(this IExpenseRepository expRepo, DateRange range) =>
        expRepo.DeleteMany(expRepo.GetExpensesByDateRange(range));

    private static Expense[] GetExpensesByDateRange(this IExpenseRepository expRepo, DateRange range) =>
        expRepo.GetEntities().Where(x => range.InRange(x.CommittedDate)).ToArray();
}
