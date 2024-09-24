using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal static class IncomeRepositoryExtensions
{
    public static Income[] GetIncomeToReconcile(
        this IIncomeRepository incRepo,
        DateTimeOffset start,
        DateTimeOffset end) =>
        incRepo.GetEntities().Where(x => x.DepositDate >= start && x.DepositDate < end).ToArray();

    public static Maybe<Income[]> DeleteByDateRange(
        this IIncomeRepository incRepo,
        DateTimeOffset start,
        DateTimeOffset end) =>
        incRepo.DeleteMany(incRepo.GetIncomeToReconcile(start, end));
}
