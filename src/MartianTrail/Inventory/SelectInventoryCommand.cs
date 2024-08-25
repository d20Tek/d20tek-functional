using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using Spectre.Console;

namespace MartianTrail.Inventory;

internal static class SelectInventoryCommand
{
    public static InventoryState SelectInitialInventory(IAnsiConsole console) =>
        console.Tap(x => x.DisplayHeader(Constants.Inventory.SelectionHeading))
               .Map(x => new InventorySelectionState(Credits: Constants.Inventory.StartingCredits))
               .Map(s => s.IterateUntil(
                   x => Constants.Inventory.InventorySelections.Aggregate(x, (acc, y) =>
                            MakeInventorySelection(console, acc, y.Name, y.CostPerItem, y.UpdateFunc))
                                .Map(y => UpdateUserIsHappyStatus(console, y)),
                   e => e.PlayerIsHappyWithSelection));

    public static InventoryState PurchaseInventory(InventoryState state, IAnsiConsole console) =>
        state.Tap(x => console.DisplayHeader(Constants.Inventory.PurchaseHeading))
             .Map(o => InventorySelectionState.From(o))
             .Map(s => s.IterateUntil(
                   x => Constants.Inventory.InventorySelections.Aggregate(x, (acc, y) =>
                            MakeInventorySelection(console, acc, y.Name, y.CostPerItem, y.UpdateFunc))
                                .Map(y => UpdateUserIsHappyStatus(console, y, s)),
                   e => e.PlayerIsHappyWithSelection));

    private static InventorySelectionState MakeInventorySelection(
        IAnsiConsole console,
        InventorySelectionState oldState,
        string name,
        int costPerItem,
        Func<int, int, InventorySelectionState, InventorySelectionState> updateFunc)
    {
        var bought = (oldState.Credits / costPerItem)
            .Map(afford => console.Ask<int>(Constants.Inventory.AmountPurchaseLabel(name, costPerItem, afford))
                .Map(attempt => attempt.IterateUntil(
                    x => Constants.Inventory.MessageForItemAmount(x, oldState.Credits * costPerItem, attempt)
                            .Tap(m => console.WriteMessage(m))
                            .Map(m => ConfirmOrRetryAmount(console, x, afford)),
                    x => ValidateUserChoice(x, afford))));

        return updateFunc(bought, oldState.Credits - (bought * costPerItem), oldState);
    }

    private static bool ValidateUserChoice(int x, int affordable) => x >= 0 && x <= affordable;

    private static int ConfirmOrRetryAmount(IAnsiConsole console, int amount, int afford) =>
        ValidateUserChoice(amount, afford) ? amount : console.Ask<int>(Constants.Inventory.ReenterAmountMsg);

    private static InventorySelectionState UpdateUserIsHappyStatus(
        IAnsiConsole console,
        InventorySelectionState invState) =>
        console.Tap(x => Constants.Inventory.DisplayPurchasedItems(invState, x))
               .Map(x => console.Confirm(Constants.Inventory.ConfirmPurchaseMsg))
               .Map(r => r ? invState with { PlayerIsHappyWithSelection = true }
                           : new(PlayerIsHappyWithSelection: false, Credits: Constants.Inventory.StartingCredits));

    private static InventorySelectionState UpdateUserIsHappyStatus(
        IAnsiConsole console,
        InventorySelectionState invState,
        InventorySelectionState prevInventory) =>
        console.Tap(x => Constants.Inventory.DisplayPurchasedItems(invState, x))
               .Map(x => console.Confirm(Constants.Inventory.ConfirmPurchaseMsg))
               .Map(r => r ? invState with
                                {
                                    Batteries = prevInventory.Batteries + invState.Batteries,
                                    Food = prevInventory.Food + invState.Food,
                                    Furs = prevInventory.Furs + invState.Furs,
                                    LaserCharges = prevInventory.LaserCharges + invState.LaserCharges,
                                    AtmosphereSuits = prevInventory.AtmosphereSuits + invState.AtmosphereSuits,
                                    MediPacks = prevInventory.MediPacks + invState.MediPacks,
                                    Credits = invState.Credits,
                                    PlayerIsHappyWithSelection = true
                                }
                           : prevInventory with { PlayerIsHappyWithSelection = false });
}
