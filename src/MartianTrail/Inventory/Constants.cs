using MartianTrail.Inventory;

namespace MartianTrail;

internal static partial class Constants
{
    public static class Inventory
    {
        public const int StartingCredits = 1000;
        public const string SelectionHeading = "Inventory Selection";
        public const string ReenterAmountMsg = "Please enter another amount...";
        public const string ConfirmPurchaseMsg = "Are you happy with these purchases?";
        private const int _defaultCostPerItem = 50;

        public static readonly IEnumerable<InventoryConfiguration> InventorySelections =
        [
            new InventoryConfiguration("Batteries", _defaultCostPerItem, (q, c, oldState) =>
                oldState with { Batteries = q, Credits = c }),
            new InventoryConfiguration("Food Packs", _defaultCostPerItem, (q, c, oldState) =>
                oldState with { Food = q, Credits = c }),
            new InventoryConfiguration("Laser Charges ", _defaultCostPerItem, (q, c, oldState) =>
                oldState with { LaserCharges = q, Credits = c }),
            new InventoryConfiguration("Atmosphere Suits", _defaultCostPerItem, (q, c, oldState) =>
                oldState with { AtmosphereSuits = q, Credits = c }),
            new InventoryConfiguration("MediPacks", _defaultCostPerItem, (q, c, oldState) =>
                oldState with { MediPacks = q, Credits = c })
        ];

        public static string AmountPurchaseLabel(string name, int costPerItem, int afford) =>
            $"{name}: cost {costPerItem} per item. You can afford {afford}.\r\nHow many would you like?";

        public static string MessageForItemAmount(int amount, int credits, int totalCost) =>
            amount < 0
                ? "That was less than zero"
                : totalCost > credits
                    ? "You can't accord that many!"
                    : "Thank you!";
    }
}
