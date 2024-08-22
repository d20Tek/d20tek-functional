using MartianTrail.Inventory;

namespace MartianTrail;

internal sealed record GameState (
    int DistanceTraveled,
    bool ReachedDestination,
    bool PlayerIsDead,
    int CurrentSol,
    PlayerActions UserActionSelectedThisTurn,
    InventoryState Inventory);
