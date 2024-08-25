using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using MartianTrail.GamePhases;
using Spectre.Console;

namespace MartianTrail.Inventory;

internal static class SellFursCommand
{
    public static InventoryState Handle(InventoryState state, IAnsiConsole console) =>
        state.Furs
            .Tap(f => console.WriteMessage($"You have {f} furs to sell."))
            .Map(f => console.Ask<int>("How many would you like to sell?"))
            .Map(sold =>  state with
            {
                    Credits = state.Credits + GameCalculations.CalculateCreditsFromFurs(sold),
                    Furs = state.Furs - sold
            });
}
