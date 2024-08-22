using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using Spectre.Console;

namespace MartianTrail.Inventory;

internal record InventoryState(
    int Batteries,
    int Food,
    int Furs,
    int LaserCharges,
    int AtmosphereSuits,
    int MediPacks,
    int Credits)
{
    public virtual void Display(IAnsiConsole console) =>
        console.Tap(x => x.DisplayHeader("Inventory Items"))
               .Tap(x => x.WriteMessage(
                    "Batteries: " + Batteries,
                    "Food Packs: " + Food,
                    "Furs: " + Furs,
                    "Laser Charges: " + LaserCharges,
                    "Atmosphere Suits: " + AtmosphereSuits,
                    "MediPacks: " + MediPacks,
                    "Remaining Credits: " + Credits,
                    ""));
}
