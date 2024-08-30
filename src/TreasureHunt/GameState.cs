namespace TreasureHunt;

internal sealed record GameState(
    Location[] TreasureLocations,
    int CurrentRoom,
    int Moves,
    string[] LatestMove)
{
    public static GameState Initialize(Location[] treasureLocations, int room) =>
        new(treasureLocations, room, 1, []);

    public Location[] GetAllTreasureInRoom(int room) =>
        TreasureLocations.Where(x => x.Room == room).ToArray();
}

internal sealed record Location(int TreasureId, int Room);