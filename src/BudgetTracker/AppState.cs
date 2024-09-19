using BudgetTracker.Persistence;
using Spectre.Console;

namespace BudgetTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    ICategoryRepository CategoryRepo,
    string Command,
    bool CanContinue)
{
    public static AppState Init(IAnsiConsole console, ICategoryRepository categoryRepo) =>
        new(console, categoryRepo, string.Empty, true);
}
