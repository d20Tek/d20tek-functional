using Spectre.Console;

namespace WealthTracker;

internal sealed record AppState(IAnsiConsole Console, string Command, bool CanContinue)
{
    public static AppState Initialize(IAnsiConsole console) => new(console, string.Empty, true);
}
