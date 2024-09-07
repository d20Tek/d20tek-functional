using Spectre.Console;

namespace WealthTracker;

internal sealed record AppState(IAnsiConsole Console, string Command = "", bool CanContinue = true);
