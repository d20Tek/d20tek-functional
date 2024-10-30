using D20Tek.Functional;
using Spectre.Console;

namespace FirePlace;

internal sealed record GameState(
    int[,] FireMatrix,
    IAnsiConsole Console,
    bool GameRunning = true) : IState
{
    public static GameState Empty() =>
        new(new int[Constants.Width, Constants.Height], AnsiConsole.Console);
}
