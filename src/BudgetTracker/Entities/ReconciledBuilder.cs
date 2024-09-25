using Apps.Common;
using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Entities;

internal static class ReconciledBuilder
{
    public static ReconciledSnapshot GenerateSnapshot(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        IIncomeRepository incRepo,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
            ReconcileState.Initialize(startDate, endDate)
                .Map(s => s.CalculateIncome(incRepo))
                .Map(s => s.CalculateGroupedExpenses(catRepo, expRepo))
                .Map(s => s.MapToSnapshot(startDate, endDate));

    private static ReconcileState CalculateIncome(this ReconcileState state, IIncomeRepository incRepo) =>
        incRepo.GetIncomeToReconcile(state.GetDateRange())
            .Map(incomes => incomes.Select(x => new ReconciledIncome(x.Name, x.Amount)))
            .Map(reconciled => state with
            {
                Incomes = reconciled.ToArray(),
                TotalIncome = new(Constants.TotalIncomeLabel, reconciled.Sum(x => x.Amount))
            });

    private static ReconcileState CalculateGroupedExpenses(
        this ReconcileState state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        state.CalcReconciledExpenses(catRepo, expRepo)
            .Map(expenses => CalcTotalExpenses(expenses)
                .Map(total => state with { Expenses = [.. expenses], TotalExpenses = total }));

    private static ReconciledExpenses[] CalcReconciledExpenses(
        this ReconcileState state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        catRepo.GetEntities()
            .Select(cat =>
                expRepo.GetExpensesToReconcile(cat.Id, new DateRange(state.StartDate,state.EndDate)).Sum(e => e.Actual)
                    .Map(a => new ReconciledExpenses(cat.Name, cat.BudgetedAmount, a, cat.BudgetedAmount - a)))
            .ToArray();

    private static ReconciledExpenses CalcTotalExpenses(ReconciledExpenses[] exp) =>
        new(Constants.TotalExpensesLabel, exp.Sum(x => x.Budget), exp.Sum(x => x.Actual), exp.Sum(x => x.Remaining));

    private static ReconciledSnapshot MapToSnapshot(this ReconcileState s, DateTimeOffset start, DateTimeOffset end) =>
        new(0, start, end, s.Incomes, s.TotalIncome, s.Expenses, s.TotalExpenses);
}
