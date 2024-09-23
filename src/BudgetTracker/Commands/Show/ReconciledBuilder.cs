using BudgetTracker.Entities;
using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Commands.Show;

internal static class ReconciledBuilder
{
    public static ReconciledSnapshot GenerateSnapshot(
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        IIncomeRepository incRepo,
        ICategoryRepository catRepo,
        IExpenseRepository expRepo)
    {
        var (income, total) = CalculateIncome(incRepo);
        var (expenses, totalExpense) = CalculateGroupedExpenses(catRepo, expRepo);
        return new(
            startDate,
            endDate,
            income,
            new ReconciledIncome("Total Income:", total),
            expenses,
            totalExpense);
    }
    private static (ReconciledIncome[], decimal) CalculateIncome(IIncomeRepository incRepo) =>
        incRepo.GetEntities()
            .Map(incomes => incomes.Select(x => new ReconciledIncome(x.Name, x.Amount)))
            .Map(reconciled => (reconciled.ToArray(), reconciled.Sum(x => x.Amount)));

    private static (ReconciledExpenses[], ReconciledExpenses) CalculateGroupedExpenses(
        ICategoryRepository catRepo,
        IExpenseRepository expRepo)
    {
        List<ReconciledExpenses> list = [];
        decimal totalBudget = 0, totalActual = 0, totalRemaining = 0;

        var categories = catRepo.GetEntities();
        foreach (var category in categories)
        {
            var expenses = expRepo.GetExpensesByCategory(category.Id);
            var budget = category.BudgetedAmount;
            totalBudget += budget;
            var actual = expenses.Sum(x => x.Actual);
            totalActual += actual;
            var remaining = budget - actual;
            totalRemaining += remaining;

            list.Add(new(category.Name, budget, actual, remaining));
        }

        return ([.. list], new("Total Expenses:", totalBudget, totalActual, totalRemaining));
    }
}
