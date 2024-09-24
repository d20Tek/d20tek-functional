using Apps.Repositories;
using BudgetTracker.Entities;

namespace BudgetTracker.Persistence;

internal sealed class BudgetDataStore
{
    public DataStoreElement<BudgetCategory> Categories { get; set; } = new();

    public DataStoreElement<Expense> Expenses { get; set; } = new();

    public DataStoreElement<Income> Incomes { get; set; } = new();

    public DataStoreElement<ReconciledSnapshot> CompletedSnapshots { get; set; } = new();
}
