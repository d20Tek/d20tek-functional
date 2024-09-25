using Apps.Common;

namespace BudgetTracker.Entities;

internal sealed record ReconcileState(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    ReconciledIncome[] Incomes,
    ReconciledIncome? TotalIncome,
    ReconciledExpenses[] Expenses,
    ReconciledExpenses? TotalExpenses)
{
    public static ReconcileState Initialize(DateTimeOffset startDate, DateTimeOffset endDate) =>
        new(startDate, endDate,[], null, [], null);

    internal DateRange GetDateRange() => new(StartDate, EndDate);
}
