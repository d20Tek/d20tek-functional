using BudgetTracker.Entities;

namespace BudgetTracker.Persistence;

internal static class ExpenseRepositoryExtensions
{
    public static Expense[] GetExpensesByCategory(this IExpenseRepository expRepo, int catId) =>
        expRepo.GetEntities().Where(x => x.CategoryId == catId).ToArray();
}
