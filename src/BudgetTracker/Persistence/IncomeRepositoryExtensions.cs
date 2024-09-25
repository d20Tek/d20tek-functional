using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal static class IncomeRepositoryExtensions
{
    public static Income[] GetIncomeToReconcile(this IIncomeRepository incRepo, DateRange range) =>
        incRepo.GetEntities().Where(x => range.InRange(x.DepositDate)).ToArray();

    public static Maybe<Income[]> DeleteByDateRange(this IIncomeRepository incRepo, DateRange range) =>
        incRepo.DeleteMany(incRepo.GetIncomeToReconcile(range));
}
