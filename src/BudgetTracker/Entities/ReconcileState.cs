using Apps.Common;

namespace BudgetTracker.Entities;

internal sealed record ReconcileState(
    DateRange Range,
    ReconciledIncome[] Incomes,
    ReconciledIncome? TotalIncome,
    ReconciledExpenses[] Expenses,
    ReconciledExpenses? TotalExpenses)
{
    public static ReconcileState Initialize(DateRange range) =>
        new(range, [], null, [], null);
}
