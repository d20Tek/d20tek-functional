using Spectre.Console;

namespace BudgetTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    string Command,
    bool CanContinue)
{
    public static AppState Init(IAnsiConsole console) =>
        new(console, string.Empty, true);
}
