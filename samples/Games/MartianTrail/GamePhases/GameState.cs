using D20Tek.Functional;
using MartianTrail.Inventory;

namespace MartianTrail.GamePhases;

internal sealed record GameState(
    int DistanceTraveled,
    int DistanceThisTurn,
    bool ReachedDestination,
    bool PlayerIsDead,
    int CurrentSol,
    PlayerActions UserActionSelectedThisTurn,
    InventoryState Inventory,
    string[] LatestMoves) : IState;
