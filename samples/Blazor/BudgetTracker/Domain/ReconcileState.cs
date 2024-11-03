using D20Tek.Functional;

namespace BudgetTracker.Domain;

internal sealed record ReconcileState(
    DateRange Range,
    ReconciledIncome[] Incomes,
    ReconciledIncome? TotalIncome,
    ReconciledExpenses[] Expenses,
    ReconciledExpenses? TotalExpenses) : IState
{
    public static ReconcileState Initialize(DateRange range) =>
        new(range, [], null, [], null);
}
