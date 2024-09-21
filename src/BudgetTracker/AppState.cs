using Spectre.Console;

namespace BudgetTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    ICategoryRepository CategoryRepo,
    IExpenseRepository ExpenseRepo,
    IIncomeRepository IncomeRepo,
    string Command,
    bool CanContinue)
{
    public static AppState Init(
        IAnsiConsole console,
        ICategoryRepository categoryRepo,
        IExpenseRepository expenseRepo,
        IIncomeRepository incomeRepo) =>
            new(console, categoryRepo, expenseRepo, incomeRepo, string.Empty, true);
}
