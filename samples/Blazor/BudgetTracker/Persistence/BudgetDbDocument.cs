using BudgetTracker.Domain;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class BudgetDbDocument : DbDocument
{
    public AutoIdEntity<BudgetCategory> Categories { get; set; } = new();

    //public AutoIdEntity<Expense> Expenses { get; set; } = new();

    //public AutoIdEntity<Income> Incomes { get; set; } = new();

    //public AutoIdEntity<ReconciledSnapshot> CompletedSnapshots { get; set; } = new();
}
