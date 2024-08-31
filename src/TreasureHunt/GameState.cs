using TreasureHunt.Data;

namespace TreasureHunt;

internal sealed record GameState(
    Location[] TreasureLocations,
    int CurrentRoom,
    int Carrying,
    int Moves,
    string[] LatestMove)
{
    public static GameState Initialize(Location[] treasureLocations, int room) =>
        new(treasureLocations, room, Constants.NoTreasure, 0, []);
}
