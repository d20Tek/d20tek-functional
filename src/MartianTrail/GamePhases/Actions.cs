using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using MartianTrail.MiniGame;
using Spectre.Console;

namespace MartianTrail.GamePhases;

internal static class Actions
{
    public static GameState DoHuntingForFood(GameState state, IAnsiConsole console, MiniGameCommand miniGame) =>
        Constants.SelectAction.HuntingFoodLabel
            .Tap(l => console.WriteMessage(l))
            .Map(l => miniGame.Play())
            .Tap(a => console.WriteMessage(Constants.SelectAction.FoodAccuracyMessage(a)))
            .Map(a => state with
            {
                Inventory = state.Inventory with
                {
                    LaserCharges = state.Inventory.LaserCharges - GameCalculations.CalculateChargesUsed(a),
                    Food = state.Inventory.Food + GameCalculations.CalculateFoodGained(a)
                }
            });

    public static GameState DoHuntingForFurs(GameState state, IAnsiConsole console, MiniGameCommand miniGame) => state;

    public static GameState DoTrading(GameState state, IAnsiConsole console, MiniGameCommand miniGame) => state;

    public static GameState DoPushOn(GameState state, IAnsiConsole console, MiniGameCommand miniGame) => state;
}
