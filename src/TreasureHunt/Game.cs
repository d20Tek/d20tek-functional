using D20Tek.Functional;
using C = TreasureHunt.Commands;
using Games.Common;
using Spectre.Console;
using TreasureHunt.Data;

namespace TreasureHunt;

internal static class Game
{
    public delegate int RndFunc(int max);

    public static void Play(IAnsiConsole console, RndFunc rnd) =>
        InitializeGame(console, rnd)
            .Map(initialState =>
                initialState.IterateUntil(
                    x => x.NextCommand(console),
                    x => x.IsGameComplete())
            .Iter(x => console.WriteMessage(x.GetEndGameMessage())));

    private static GameState InitializeGame(IAnsiConsole console, RndFunc rnd) =>
        GameState.Initialize(GameData.GetTreasureLocations(), rnd(Constants.TotalRooms))
                .Iter(s => console.Write(Presenters.GameHeader(Constants.GameTitle)))
                .Iter(x => console.DisplayInstructions(
                    Constants.ShowInstructionsLabel,
                    Constants.Instructions,
                    Constants.StartGameLabel));

    private static GameState NextCommand(this GameState state, IAnsiConsole console) =>
        state.Iter(s => s.DisplayCurrentLocation(console))
             .Map(s => C.Commands.ProcessCommand(s, Inputs.GetCommand(console)))
             .Iter(s => s.DisplayLatestMoves(console));

    private static bool IsGameComplete(this GameState state) =>
        state.TreasureLocations.First().Room
             .ToIdentity()
             .Map(first => state.TreasureLocations.All(x => x.Room == first) || 
                           state.Moves > Constants.MovesAllowed);

    private static string[] GetEndGameMessage(this GameState endState) =>
        (endState.Moves > Constants.MovesAllowed)
            ? [Constants.EndGameLostMessage]
            : Constants.EndGameWonMessage(endState.CurrentRoom, endState.Moves);
}
