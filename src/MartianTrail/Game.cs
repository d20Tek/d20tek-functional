using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using MartianTrail.Inventory;
using Spectre.Console;

namespace MartianTrail;

internal static class Game
{
    public static void Run(IAnsiConsole console)
    {
        var initialState = InitializeGame(console);
        /*var finalState = initialState
            .IterateUntil(
                x => StateMachine.NextState(x, rollFunc)
                        .Tap(y => AnsiConsole.MarkupLine($"[{x.GetCurrentPlayer().Color}]{y.LatestMove}[/]")),
                x => x.Players.Any(y => y.IsWinner()))
            .Tap(x => DisplayWinner(console, x));*/
    }

    private static GameState InitializeGame(IAnsiConsole console) =>
        console.Tap(x => DisplayTitle(x))
               .Tap(x => DisplayInstructions(x))
               .Map(x => SelectInitialInventoryCommand.SelectInitialInventory(x))
               .Map(i => StateMachine.InitialState(i));

    private static void DisplayTitle(IAnsiConsole console) =>
        new FigletText("Martian Trail")
            .Centered()
            .Color(Color.Green)
            .Tap(x => console.Write(x));

    private static void DisplayInstructions(IAnsiConsole console) =>
        console.Confirm("Would you like the game instructions?")
            .Tap(x => console.WriteMessageConditional(x, Constants.Instructions));
}
