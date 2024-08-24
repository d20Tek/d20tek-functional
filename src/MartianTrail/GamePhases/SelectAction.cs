using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using MartianTrail.MiniGame;
using Spectre.Console;

namespace MartianTrail.GamePhases;

internal sealed class SelectAction : IGamePhase
{
    private readonly Func<int> _rndPercentage;
    private readonly IAnsiConsole _console;
    private readonly MiniGameCommand _playMiniGame;


    public SelectAction(Func<int, int> rnd, IAnsiConsole console, MiniGameCommand playMiniGame)
    {
        _rndPercentage = () => rnd(100);
        _console = console;
        _playMiniGame = playMiniGame;
    }

    public GameState DoPhase(GameState oldState) =>
        CalculateLocationFlags(_rndPercentage)
            .Map(f => CalculatePlayerOptions(_rndPercentage, f)
                .Tap(options => _console.WriteMessage(Constants.SelectAction.PlayerActionsMessage(options, f.IsWilderness)))
                .Map(options => GetSelectedAction(_console, options))
                .Map(action => GetActionFunc(action)
                    .Map(f => f(oldState) with
                    {
                        UserActionSelectedThisTurn = action,
                        LatestMoves = []
                    })));

    private static PlayerActions GetSelectedAction(IAnsiConsole console, PlayerActionOptions[] options) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.SelectAction.PlayerChoiceLabel)
                                .AddChoices(options.Select(x => x.ChoiceNumber)))
            .Map(c => options.Single(x => x.ChoiceNumber == c).Action);

    private static PlayerActionOptions[] CalculatePlayerOptions(Func<int> rnd, LocationFlags flags) =>
        new[]
        {
            flags.IsTradingPost ? PlayerActions.TradeAtOutpost : PlayerActions.Unavailable,
            flags.IsHuntingArea && GameCalculations.IsHuntingAvailable(rnd) ? PlayerActions.HuntForFood : PlayerActions.Unavailable,
            flags.IsHuntingArea && GameCalculations.IsHuntingAvailable(rnd) ? PlayerActions.HuntForSkins : PlayerActions.Unavailable,
            PlayerActions.PushOn
        }.Where(x => x != PlayerActions.Unavailable)
            .Select((x, i) => new PlayerActionOptions(
                x,
                i + 1))
            .ToArray();

    private Func<GameState, GameState> GetActionFunc(PlayerActions actionToDo) =>
        actionToDo switch
        {
            PlayerActions.TradeAtOutpost => DoTrading,
            PlayerActions.HuntForFood => DoHuntingForFood,
            PlayerActions.HuntForSkins => DoHuntingForFurs,
            _ => DoPushOn,
        };

    private GameState DoHuntingForFood(GameState state) =>
        Constants.SelectAction.HuntingFoodLabel
            .Tap(l => _console.WriteMessage(l))
            .Map(l => _playMiniGame.Play())
            .Tap(a => _console.WriteMessage(Constants.SelectAction.FoodAccuracyMessage(a)))
            .Map(a => state with
            {
                Inventory = state.Inventory with
                {
                    LaserCharges = state.Inventory.LaserCharges - GameCalculations.CalculateChargesUsed(a),
                    Food = state.Inventory.Food + GameCalculations.CalculateFoodGained(a)
                }
            });

    private static LocationFlags CalculateLocationFlags(Func<int> rnd) =>
        GameCalculations.IsWilderness(rnd)
            .Map(isWilderness =>
                new LocationFlags(
                    GameCalculations.IsWilderness(rnd),
                    GameCalculations.IsTradingPost(rnd, isWilderness),
                    GameCalculations.IsHuntingArea(rnd, isWilderness)));

    public sealed record LocationFlags(bool IsWilderness, bool IsTradingPost, bool IsHuntingArea);

    private static GameState DoHuntingForFurs(GameState state) => state;

    private static GameState DoTrading(GameState state) => state;

    private static GameState DoPushOn(GameState state) => state;
}
