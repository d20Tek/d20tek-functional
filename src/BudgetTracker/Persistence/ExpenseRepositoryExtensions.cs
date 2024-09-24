using BudgetTracker.Entities;

namespace BudgetTracker.Persistence;

internal static class ExpenseRepositoryExtensions
{
    public static Expense[] GetExpensesToReconcile(
        this IExpenseRepository expRepo,
        int catId,
        DateTimeOffset start,
        DateTimeOffset end) =>
        expRepo.GetEntities().Where(x => x.CategoryId == catId)
                             .Where(x => x.CommittedDate >= start && x.CommittedDate < end)
                             .ToArray();
}
