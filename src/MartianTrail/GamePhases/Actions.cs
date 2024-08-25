using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using MartianTrail.Inventory;
using MartianTrail.MiniGame;
using Spectre.Console;

namespace MartianTrail.GamePhases;

internal static class Actions
{
    public static GameState DoHuntingForFood(GameState state, IAnsiConsole console, MiniGameCommand miniGame) =>
        state.PlayerCanHuntAlt(
            cannotHunt: (x) => x.Tap(y => console.WriteMessage(Constants.SelectAction.CannotHuntLabel)),
            canHunt: (s) =>
                s.Tap(s => console.WriteMessage(Constants.SelectAction.HuntingFoodLabel))
                 .Map(s => miniGame.Play())
                 .Tap(a => console.WriteMessage(Constants.SelectAction.FoodAccuracyMessage(a)))
                 .Map(a => s with
                 {
                    Inventory = s.Inventory with
                    {
                        LaserCharges = (s.Inventory.LaserCharges - GameCalculations.CalculateChargesUsed(a))
                                         .Map(x => x >= 0 ? x : 0),
                        Food = s.Inventory.Food + GameCalculations.CalculateFoodGained(a)
                    }
                 }));

    public static GameState DoHuntingForFurs(GameState state, IAnsiConsole console, MiniGameCommand miniGame) =>
        state.PlayerCanHuntAlt(
            cannotHunt: (x) => x.Tap(y => console.WriteMessage(Constants.SelectAction.CannotHuntLabel)),
            canHunt: (s) => 
                s.Tap(s => console.WriteMessage(Constants.SelectAction.HuntingFursLabel))
                 .Map(s => miniGame.Play())
                 .Tap(a => console.WriteMessage(Constants.SelectAction.FursAccuracyMessage(a)))
                 .Map(a => s with
                 {
                    Inventory = s.Inventory with
                    {
                        LaserCharges = (s.Inventory.LaserCharges - GameCalculations.CalculateChargesUsed(a))
                                         .Map(x => x >= 0 ? x : 0),
                        Furs = s.Inventory.Furs + GameCalculations.CalculateFursGained(a)
                    }
                 }));

    public static GameState DoTrading(GameState state, IAnsiConsole console, MiniGameCommand miniGame) =>
        state.Tap(s => console.WriteMessage(Constants.SelectAction.TradingPostLabel))
             .Map(s => SellFursCommand.Handle(s.Inventory, console)
                 .Map(sold => SelectInventoryCommand.PurchaseInventory(sold, console)
                     .Map(bought => s with { Inventory = bought })));

    public static GameState DoPushOn(GameState state, IAnsiConsole console, MiniGameCommand miniGame) => state;

    private static GameState PlayerCanHuntAlt(
        this GameState state,
        Func<GameState, GameState> cannotHunt,
        Func<GameState, GameState> canHunt) =>
        state.PlayerCanHunt() ? canHunt(state) : cannotHunt(state);

}
