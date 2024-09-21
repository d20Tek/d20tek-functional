using Spectre.Console;

namespace BudgetTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    ICategoryRepository CategoryRepo,
    IExpenseRepository ExpenseRepo,
    ICreditRepository CreditRepo,
    string Command,
    bool CanContinue)
{
    public static AppState Init(
        IAnsiConsole console,
        ICategoryRepository categoryRepo,
        IExpenseRepository expenseRepo,
        ICreditRepository creditRepo) =>
            new(console, categoryRepo, expenseRepo, creditRepo, string.Empty, true);
}
