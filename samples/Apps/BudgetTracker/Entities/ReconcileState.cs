using Apps.Common;
using D20Tek.Functional;

namespace BudgetTracker.Entities;

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
