using Apps.Repositories;
using D20Tek.Functional;

namespace BudgetTracker.Domain;

internal sealed class ReconciledSnapshot : IEntity, IState
{
    public int Id { get; set; }

    public DateTimeOffset StartDate { get; }

    public DateTimeOffset EndDate { get; }

    public ReconciledIncome[] Income { get; }

    public ReconciledIncome TotalIncome { get; }
    
    public ReconciledExpenses[] CategoryExpenses { get; }

    public ReconciledExpenses TotalExpenses { get; }

    public ReconciledSnapshot(
        int id,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        ReconciledIncome[] income,
        ReconciledIncome? totalIncome,
        ReconciledExpenses[] categoryExpenses,
        ReconciledExpenses? totalExpenses)
    {
        ArgumentNullException.ThrowIfNull(totalIncome, nameof(totalIncome));
        ArgumentNullException.ThrowIfNull(totalExpenses, nameof(totalExpenses));

        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Income = income;
        TotalIncome = totalIncome;
        CategoryExpenses = categoryExpenses;
        TotalExpenses = totalExpenses;
    }

    public void SetId(int id) => Id = id;

    internal DateRange GetDateRange() => new(StartDate, EndDate);
}

internal sealed record ReconciledIncome(string Name, decimal Amount);

internal sealed record ReconciledExpenses(string Category, decimal Budget, decimal Actual, decimal Remaining);
