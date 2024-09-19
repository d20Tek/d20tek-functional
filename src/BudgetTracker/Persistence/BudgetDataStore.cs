using Apps.Repositories;
using BudgetTracker.Entities;

namespace BudgetTracker.Persistence;

internal sealed class BudgetDataStore
{
    public DataStoreElement<BudgetCategory> Categories { get; set; } = new();
}
