using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace Crash;

internal static class Game
{
    public delegate int RndFunc(int max);

    public static void Play(IAnsiConsole console, RndFunc rnd) =>
        InitializeGame(console)
            .Map(initialState =>
                initialState.IterateUntil(
                    x => x.NextRound(console, rnd),
                    x => x.IsGameComplete())
            .Iter(x =>
            {
                console.Clear();
                console.WriteMessage(x.GetEndGameMessage());
            }));

    private static GameState InitializeGame(IAnsiConsole console) =>
        GameState.Empty()
                 .Iter(s => console.Write(Presenters.GameHeader(Constants.GameTitle)))
                 .Iter(x => console.DisplayInstructions(
                     Constants.ShowInstructionsLabel,
                     Constants.Instructions,
                     Constants.StartGameLabel))
                 .Iter(x => console.Clear());

    private static GameState NextRound(this GameState state, IAnsiConsole console, RndFunc rnd) =>
        GameRound.PlayRound(state, console, rnd);

    private static bool IsGameComplete(this GameState state) => state.KeepPlaying is false;

    private static string[] GetEndGameMessage(this GameState endState) =>
        endState.ConsoleSizeError 
            ? [..Constants.ConsoleSizeError, Constants.EndGameMessage]
            : [Constants.EndGameMessage];
}
