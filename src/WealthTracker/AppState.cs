using Spectre.Console;

namespace WealthTracker;

internal sealed record AppState(
    IAnsiConsole Console,
    IWealthRepository Repository,
    string Command,
    bool CanContinue)
{
    public static AppState Initialize(IAnsiConsole console, IWealthRepository repo) =>
        new(console, repo, string.Empty, true);
}
