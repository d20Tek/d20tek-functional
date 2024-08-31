using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;
using TreasureHunt.Data;

namespace TreasureHunt;

internal static class Presenter
{
    public static void DisplayCurrentLocation(this GameState state, IAnsiConsole console) =>
        state.Apply(s => console.WriteMessage(Constants.RoomDescription(s.CurrentRoom)))
             .Map(s => s.GetAllTreasureInRoom(s.CurrentRoom))
             .Apply(l => console.WriteMessage(Constants.TreasureDescriptions(l)));

    public static void DisplayLatestMoves(this GameState state, IAnsiConsole console) =>
        console.WriteMessage(state.LatestMove);

    private static Location[] GetAllTreasureInRoom(this GameState state, int room) =>
        state.TreasureLocations.Where(x => x.Room == room).ToArray();
}
