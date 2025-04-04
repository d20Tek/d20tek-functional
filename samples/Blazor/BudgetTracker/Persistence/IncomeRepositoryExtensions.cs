﻿using BudgetTracker.Domain;
using D20Tek.Functional;

namespace BudgetTracker.Persistence;

internal static class IncomeRepositoryExtensions
{
    public static Income[] GetIncomeToReconcile(this IIncomeRepository incRepo, DateRange range) =>
        incRepo.GetEntities().Where(x => range.InRange(x.DepositDate)).ToArray();

    public static Result<Income[]> DeleteByDateRange(this IIncomeRepository incRepo, DateRange range) =>
        incRepo.DeleteMany(incRepo.GetIncomeToReconcile(range));
}
