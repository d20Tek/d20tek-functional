using D20Tek.Functional;
using Spectre.Console;

namespace ChutesAndLadders;

internal static class Game
{
    public static void Run(IAnsiConsole console, Func<int> rollFunc)
    {
        var initialState = InitializeGame(console);
        var finalState = initialState
            .IterateUntil(
                x => StateMachine.NextState(x, rollFunc)
                        .Iter(y => AnsiConsole.MarkupLine($"[{x.GetCurrentPlayer().Color}]{y.LatestMove}[/]")),
                x => x.Players.Any(y => y.IsWinner()))
            .Iter(x => DisplayWinner(console, x));
    }

    private static GameState InitializeGame(IAnsiConsole console) =>
        console.ToIdentity()
               .Iter(DisplayTitle)
               .Map(GetNumberOfPlayers)
               .Map(StateMachine.InitialState);

    private static void DisplayTitle(IAnsiConsole console) =>
        new FigletText("Chutes & Ladders").Centered()
                                          .Color(Color.Green)
                                          .ToIdentity()
                                          .Iter(console.Write);

    private static int GetNumberOfPlayers(IAnsiConsole console) =>
        console.Prompt(
            new TextPrompt<int>("Enter the number of players (2-8):")
                .Validate(x => x >= 2 && x <= 8, "Number of players must be between 2 and 8."));

    private static void DisplayWinner(IAnsiConsole console, GameState finalState) =>
        console.MarkupLine(finalState.GetWinningPlayer() switch
        {
            Optional<Player> p when p.IsSome => $"The winner is [{p.Get().Color}]Player {p.Get().Number}[/]!!!",
            _ => $"[red]Error:[/] There was an unexpected error running through this game."
        });
}
