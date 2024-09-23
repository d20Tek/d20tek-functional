namespace BudgetTracker.Entities;

internal sealed record ReconciledSnapshot(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    ReconciledIncome[] Income,
    ReconciledIncome TotalIncome,
    ReconciledExpenses[] CategoryExpenses,
    ReconciledExpenses TotalExpenses);

internal sealed record ReconciledIncome(string Name, decimal Amount);

internal sealed record ReconciledExpenses(string Category, decimal Budget, decimal Actual, decimal Remaining);
