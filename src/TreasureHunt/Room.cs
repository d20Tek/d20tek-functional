namespace TreasureHunt;

internal sealed record Room(
    int Id,
    int North,
    int East,
    int South,
    int West,
    string Description);
