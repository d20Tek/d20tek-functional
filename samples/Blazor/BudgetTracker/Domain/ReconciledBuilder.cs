using BudgetTracker.Common;
using BudgetTracker.Persistence;
using D20Tek.Functional;

namespace BudgetTracker.Domain;

internal static class ReconciledBuilder
{
    public static ReconciledSnapshot GenerateSnapshot(
        DateRange range,
        IIncomeRepository incRepo,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
            ReconcileState.Initialize(range)
                .Map(s => s.CalculateIncome(incRepo))
                .Map(s => s.CalculateGroupedExpenses(catRepo, expRepo))
                .Map(s => s.MapToSnapshot(range));

    private static ReconcileState CalculateIncome(this ReconcileState state, IIncomeRepository incRepo) =>
        incRepo.Find(i => state.Range.InRange(i.DepositDate))
            .Map(incomes => incomes.Select(x => new ReconciledIncome(x.Name, x.Amount)))
            .Map(reconciled => state with
            {
                Incomes = reconciled.ToArray(),
                TotalIncome = new(Constants.TotalIncomeLabel, reconciled.Sum(x => x.Amount))
            }).GetValue();

    private static ReconcileState CalculateGroupedExpenses(
        this ReconcileState state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        state.CalcReconciledExpenses(catRepo, expRepo)
             .Pipe(expenses => CalcTotalExpenses(expenses)
                .Pipe(total => state with { Expenses = [.. expenses], TotalExpenses = total }));

    private static ReconciledExpenses[] CalcReconciledExpenses(
        this ReconcileState state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        catRepo.GetAll().GetValue()
               .Select(cat =>
                    expRepo.GetExpensesToReconcile(cat.Id, state.Range)
                        .Match(x => x.Sum(e => e.Actual), _ => 0)
                        .Pipe(a => new ReconciledExpenses(cat.Name, cat.BudgetedAmount, a, cat.BudgetedAmount - a)))
               .ToArray();

    private static ReconciledExpenses CalcTotalExpenses(ReconciledExpenses[] exp) =>
        new(Constants.TotalExpensesLabel, exp.Sum(x => x.Budget), exp.Sum(x => x.Actual), exp.Sum(x => x.Remaining));

    private static ReconciledSnapshot MapToSnapshot(this ReconcileState s, DateRange r) =>
        new(0, r.Start, r.End, s.Incomes, s.TotalIncome, s.Expenses, s.TotalExpenses);
}
