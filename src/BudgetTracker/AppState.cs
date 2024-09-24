using Spectre.Console;

namespace BudgetTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    ICategoryRepository CategoryRepo,
    IExpenseRepository ExpenseRepo,
    IIncomeRepository IncomeRepo,
    IReconciledSnapshotRepository SnapshotRepo,
    string Command,
    bool CanContinue)
{
    public static AppState Init(
        IAnsiConsole console,
        ICategoryRepository categoryRepo,
        IExpenseRepository expenseRepo,
        IIncomeRepository incomeRepo,
        IReconciledSnapshotRepository snapshotRepo) =>
            new(console, categoryRepo, expenseRepo, incomeRepo, snapshotRepo, string.Empty, true);
}
