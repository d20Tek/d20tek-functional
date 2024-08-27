using MartianTrail.Common;
using MartianTrail.Inventory;
using Spectre.Console;

namespace MartianTrail;

internal static partial class Constants
{
    public static class Inventory
    {
        public const int StartingCredits = 1000;
        public const string SelectionHeading = "Inventory Selection";
        public const string PurchaseHeading = "Purchase Inventory Selection";
        public const string ReenterAmountMsg = "Please enter another amount...";
        public const string ConfirmPurchaseMsg = "Are you happy with these purchases?";

        public static readonly InventoryConfiguration[] InventorySelections =
        [
            new InventoryConfiguration("Batteries", 50, (q, c, oldState) =>
                oldState with { Batteries = q, Credits = c }),
            new InventoryConfiguration("Food Packs", 10, (q, c, oldState) =>
                oldState with { Food = q, Credits = c }),
            new InventoryConfiguration("Laser Charges ", 40, (q, c, oldState) =>
                oldState with { LaserCharges = q, Credits = c }),
            new InventoryConfiguration("Atmosphere Suits", 15, (q, c, oldState) =>
                oldState with { AtmosphereSuits = q, Credits = c }),
            new InventoryConfiguration("MediPacks", 30, (q, c, oldState) =>
                oldState with { MediPacks = q, Credits = c })
        ];

        public static readonly InventoryConfiguration[] YardSaleSelections =
        [
            new InventoryConfiguration("Batteries", 10, (q, c, oldState) =>
                oldState with { Batteries = q, Credits = c }),
            new InventoryConfiguration("Atmosphere Suits", 2, (q, c, oldState) =>
                oldState with { AtmosphereSuits = q, Credits = c }),
        ];

        public static string AmountPurchaseLabel(string name, int costPerItem, int afford) =>
            $"{name}: cost {costPerItem} per item. You can afford {afford}.\r\nHow many would you like?";

        public static string MessageForItemAmount(int amount, int credits, int totalCost) =>
            amount < 0
                ? "That was less than zero"
                : totalCost > credits
                    ? "You can't accord that many!"
                    : "Thank you!";

        public static void DisplayPurchasedItems(InventoryState invState, IAnsiConsole console) =>
            console.WriteMessage(
                "===== Items Purchased =====",
                "Batteries: " + invState.Batteries,
                "Food Packs: " + invState.Food,
                "Furs: " + invState.Furs,
                "Laser Charges: " + invState.LaserCharges,
                "Atmosphere Suits: " + invState.AtmosphereSuits,
                "MediPacks: " + invState.MediPacks,
                "Remaining Credits: " + invState.Credits,
                "");

        public static string SellFursMessage(int furs) => $"You have {furs} furs to sell.";
        public const string SellFursLabel = "How many would you like to sell?";
        public const string SellFursError = "You cannot sell that many furs. Please try again.";
    }
}
