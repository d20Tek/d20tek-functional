using D20Tek.Functional;
using Games.Common;
using MartianTrail.MiniGame;
using Spectre.Console;

namespace MartianTrail.GamePhases;

internal sealed class SelectAction(Func<int, int> rnd, IAnsiConsole console, MiniGameCommand playMiniGame) : IGamePhase
{
    private readonly Func<int> _rndPercentage = () => rnd(100);
    private readonly IAnsiConsole _console = console;
    private readonly MiniGameCommand _playMiniGame = playMiniGame;

    public GameState DoPhase(GameState oldState) =>
        GameCalculations.CalculateLocationFlags(_rndPercentage)
            .ToIdentity()
            .Map(f => GameCalculations.CalculatePlayerOptions(_rndPercentage, f)
                .ToIdentity()
                .Map(options => GetSelectedAction(_console, options, f.IsWilderness))
                .Map(action => GetActionFunc(action)
                    .Pipe(f => f(oldState, _console, _playMiniGame) with
                    {
                        UserActionSelectedThisTurn = action,
                        LatestMoves = []
                    })).Get());

    private static PlayerActions GetSelectedAction(
        IAnsiConsole console,
        PlayerActionOptions[] options,
        bool isWilderness) =>
        options.ToIdentity()
               .Iter(o => console.WriteMessage(Constants.SelectAction.PlayerActionsMessage(o, isWilderness)))
               .Map(o => console.Prompt<int>(new TextPrompt<int>(Constants.SelectAction.PlayerChoiceLabel)
                                .AddChoices(options.Select(x => x.ChoiceNumber)))
                                .Pipe(c => options.Single(x => x.ChoiceNumber == c).Action));

    private Func<GameState, IAnsiConsole, MiniGameCommand, GameState> GetActionFunc(PlayerActions actionToDo) =>
        actionToDo switch
        {
            PlayerActions.TradeAtOutpost => Actions.DoTrading,
            PlayerActions.HuntForFood => Actions.DoHuntingForFood,
            PlayerActions.HuntForSkins => Actions.DoHuntingForFurs,
            _ => Actions.DoPushOn,
        };
}
