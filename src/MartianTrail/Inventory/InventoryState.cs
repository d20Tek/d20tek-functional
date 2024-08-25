namespace MartianTrail.Inventory;

internal record InventoryState(
    int Batteries,
    int Food,
    int Furs,
    int LaserCharges,
    int AtmosphereSuits,
    int MediPacks,
    int Credits);
