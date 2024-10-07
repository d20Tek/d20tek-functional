using D20Tek.Functional;
using Spectre.Console;

namespace TerminalAppTemplate;

internal sealed record AppState(IAnsiConsole Console, string Command, bool CanContinue) : IState
{
    public static AppState Initialize(IAnsiConsole console) => new(console, string.Empty, true);
}
