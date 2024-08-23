using MartianTrail.Inventory;

namespace MartianTrail.GamePhases;

internal sealed record GameState(
    int DistanceTraveled,
    bool ReachedDestination,
    bool PlayerIsDead,
    int CurrentSol,
    PlayerActions UserActionSelectedThisTurn,
    InventoryState Inventory,
    string[] LatestMoves);
