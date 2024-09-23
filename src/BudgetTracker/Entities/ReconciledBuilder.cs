using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Entities;

internal static class ReconciledBuilder
{
    public sealed record State(
        ReconciledIncome[] Incomes,
        ReconciledIncome? TotalIncome,
        ReconciledExpenses[] Expenses,
        ReconciledExpenses? TotalExpenses)
    {
        public static State Initialize() => new([], null, [], null);
    }

    public static ReconciledSnapshot GenerateSnapshot(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        IIncomeRepository incRepo,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
            State.Initialize()
                .Map(s => s.CalculateIncome(incRepo))
                .Map(s => s.CalculateGroupedExpenses(catRepo, expRepo))
                .Map(s => s.MapToSnapshot(startDate, endDate));

    private static State CalculateIncome(this State state, IIncomeRepository incRepo) =>
        incRepo.GetEntities()
            .Map(incomes => incomes.Select(x => new ReconciledIncome(x.Name, x.Amount)))
            .Map(reconciled => state with
            {
                Incomes = reconciled.ToArray(),
                TotalIncome = new(Constants.TotalIncomeLabel, reconciled.Sum(x => x.Amount))
            });

    private static State CalculateGroupedExpenses(
        this State state,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo) =>
        CalcReconciledExpenses(catRepo, expRepo)
            .Map(expenses => CalcTotalExpenses(expenses)
                .Map(total => state with { Expenses = [.. expenses], TotalExpenses = total }));

    private static ReconciledExpenses[] CalcReconciledExpenses(ICategoryRepository catRepo, IExpenseRepository expRepo) =>
        catRepo.GetEntities()
            .Select(cat =>
                expRepo.GetExpensesByCategory(cat.Id).Sum(e => e.Actual)
                    .Map(a => new ReconciledExpenses(cat.Name, cat.BudgetedAmount, a, cat.BudgetedAmount - a)))
            .ToArray();

    private static ReconciledExpenses CalcTotalExpenses(ReconciledExpenses[] exp) =>
        new(Constants.TotalExpensesLabel, exp.Sum(x => x.Budget), exp.Sum(x => x.Actual), exp.Sum(x => x.Remaining));

    private static ReconciledSnapshot MapToSnapshot(this State s, DateTimeOffset start, DateTimeOffset end) =>
        new(start, end, s.Incomes, s.TotalIncome!, s.Expenses, s.TotalExpenses!);
}
