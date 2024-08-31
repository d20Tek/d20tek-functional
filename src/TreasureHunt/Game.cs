using D20Tek.Minimal.Functional;
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
            .Apply(x => console.WriteMessage(x.GetEndGameMessage())));

    private static GameState InitializeGame(IAnsiConsole console, RndFunc rnd) =>
        GameState.Initialize(GameData.GetTreasureLocations(), rnd(Constants.TotalRooms))
                .Apply(s => console.Write(Presenters.GameHeader(Constants.GameTitle)))
                .Apply(x => console.DisplayInstructions(
                    Constants.ShowInstructionsLabel,
                    Constants.Instructions,
                    Constants.StartGameLabel));

    private static GameState NextCommand(this GameState state, IAnsiConsole console) =>
        state.Apply(s => s.DisplayCurrentLocation(console))
             .Map(s => C.Commands.ProcessCommand(s, Inputs.GetCommand(console)))
             .Apply(s => s.DisplayLatestMoves(console));

    private static bool IsGameComplete(this GameState state) =>
        state.TreasureLocations.First().Room
            .Map(first => state.TreasureLocations.All(x => x.Room == first) || 
                          state.Moves > Constants.MovesAllowed);

    private static string[] GetEndGameMessage(this GameState endState) =>
        (endState.Moves > Constants.MovesAllowed)
            ? [Constants.EndGameLostMessage]
            : Constants.EndGameWonMessage(endState.CurrentRoom, endState.Moves);
}
