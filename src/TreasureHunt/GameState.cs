using D20Tek.Functional;
using TreasureHunt.Data;

namespace TreasureHunt;

internal sealed record GameState(
    Location[] TreasureLocations,
    int CurrentRoom,
    int Carrying,
    int Moves,
    string[] LatestMove) : IState
{
    public static GameState Initialize(Location[] treasureLocations, int room) =>
        new(treasureLocations, room, Constants.NoTreasure, 0, []);
}
