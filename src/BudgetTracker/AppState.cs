using Spectre.Console;

namespace BudgetTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    ICategoryRepository CategoryRepo,
    IExpenseRepository ExpenseRepo,
    string Command,
    bool CanContinue)
{
    public static AppState Init(
        IAnsiConsole console,
        ICategoryRepository categoryRepo,
        IExpenseRepository expenseRepo) =>
            new(console, categoryRepo, expenseRepo, string.Empty, true);
}
