using BudgetTracker.Entities;

namespace BudgetTracker.Persistence;

internal static class IncomeRepositoryExtensions
{
    public static Income[] GetIncomeToReconcile(
        this IIncomeRepository incRepo,
        DateTimeOffset start,
        DateTimeOffset end) =>
        incRepo.GetEntities().Where(x => x.DepositDate >= start && x.DepositDate < end).ToArray();
}
