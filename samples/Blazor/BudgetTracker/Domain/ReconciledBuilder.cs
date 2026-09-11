using BudgetTracker.Common;
using D20Tek.Functional;

namespace BudgetTracker.Domain;

internal static class ReconciledBuilder
{
    public static async Task<ReconciledSnapshot> GenerateSnapshot(
        DateRange range,
        IIncomeRepository incRepo,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo)
    {
        var withIncome = await ReconcileState.Initialize(range).CalculateIncome(incRepo);
        var withExpenses = await withIncome.CalculateGroupedExpenses(catRepo, expRepo);
        return withExpenses.MapToSnapshot(range);
    }

    private static async Task<ReconcileState> CalculateIncome(this ReconcileState state, IIncomeRepository incRepo) =>
        (await incRepo.FindAsync(i => state.Range.InRange(i.DepositDate)))
            .Map(incomes => incomes.Select(x => new ReconciledIncome(x.Name, x.Amount)))
            .Map(reconciled => state with
            {
                Incomes = reconciled.ToArray(),
                TotalIncome = new(Constants.TotalIncomeLabel, reconciled.Sum(x => x.Amount))
            }).GetValue();

    private static async Task<ReconcileState> CalculateGroupedExpenses(
        this ReconcileState state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        (await state.CalcReconciledExpenses(catRepo, expRepo))
             .Pipe(expenses => CalcTotalExpenses(expenses)
                .Pipe(total => state with { Expenses = [.. expenses], TotalExpenses = total }));

    private static async Task<ReconciledExpenses[]> CalcReconciledExpenses(
        this ReconcileState state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        [.. await Task.WhenAll(
            (await catRepo.GetAllAsync()).GetValue()
               .Select(async cat =>
                    (await expRepo.GetExpensesToReconcile(cat.Id, state.Range))
                        .Match(x => x.Sum(e => e.Actual), _ => 0)
                        .Pipe(a => new ReconciledExpenses(cat.Name, cat.BudgetedAmount, a, cat.BudgetedAmount - a))))];

    private static ReconciledExpenses CalcTotalExpenses(ReconciledExpenses[] exp) =>
        new(Constants.TotalExpensesLabel, exp.Sum(x => x.Budget), exp.Sum(x => x.Actual), exp.Sum(x => x.Remaining));

    private static ReconciledSnapshot MapToSnapshot(this ReconcileState s, DateRange r) =>
        new(0, r.Start, r.End, s.Incomes, s.TotalIncome, s.Expenses, s.TotalExpenses);
}
