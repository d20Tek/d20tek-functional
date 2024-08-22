using MartianTrail.Inventory;

namespace MartianTrail;

internal static class StateMachine
{
    public static GameState InitialState(InventoryState initialInventory) => new(
        DistanceTraveled: 0,
        ReachedDestination: false,
        PlayerIsDead: false,
        CurrentSol: 0,
        UserActionSelectedThisTurn: new PlayerActions(),
        Inventory: initialInventory);
}
