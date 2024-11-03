namespace BudgetTracker.Domain;

internal sealed record DateRange(DateTimeOffset Start, DateTimeOffset End)
{
    public bool InRange(DateTimeOffset date) => date.CompareTo(Start) >= 0 && date.CompareTo(End) <= 0;
}