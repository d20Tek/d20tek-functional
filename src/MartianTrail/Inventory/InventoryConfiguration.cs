namespace MartianTrail.Inventory;

internal sealed record InventoryConfiguration(
    string Name,
    int CostPerItem,
    Func<int, int, InventorySelectionState, InventorySelectionState> UpdateFunc);
