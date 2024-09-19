using BudgetTracker.Entities;

namespace BudgetTracker.Persistence;

internal sealed class BudgetDataStore
{
    public int CategoryLastId { get; set; } = 0;

    public List<BudgetCategory> Categories { get; init; } = [];

    internal int GetNextCategoryId() => ++CategoryLastId;
}
