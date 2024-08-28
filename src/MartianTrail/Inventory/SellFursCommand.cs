﻿using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using MartianTrail.GamePhases;
using Spectre.Console;

namespace MartianTrail.Inventory;

internal static class SellFursCommand
{
    public static InventoryState Handle(InventoryState state, IAnsiConsole console) =>
        state.Furs
            .Apply(f => console.WriteMessage(Constants.Inventory.SellFursMessage(f)))
            .Map(f => console.Prompt<int>(new TextPrompt<int>(Constants.Inventory.SellFursLabel)
                                              .Validate(x=> x >= 0 && x <= f, Constants.Inventory.SellFursError)))
            .Map(sold =>  state with
            {
                Credits = state.Credits + GameCalculations.CalculateCreditsFromFurs(sold),
                Furs = (state.Furs - sold).Map(x => x >= 0 ? x : 0)
            });
}
