using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

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

    public static Maybe<Expense[]> DeleteByDateRange(
        this IExpenseRepository expRepo,
        DateTimeOffset start,
        DateTimeOffset end) =>
        expRepo.DeleteMany(expRepo.GetExpensesByDateRange(start, end).ToArray());

    private static Expense[] GetExpensesByDateRange(
        this IExpenseRepository expRepo,
        DateTimeOffset start,
        DateTimeOffset end) =>
        expRepo.GetEntities().Where(x => x.CommittedDate >= start && x.CommittedDate < end).ToArray();
}
