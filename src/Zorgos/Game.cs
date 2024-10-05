using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace Zorgos;

internal static class Game
{
    public static void Play(IAnsiConsole console, Func<int> rollFunc) =>
        InitializeGame(console)
            .Map(initialState =>
                initialState.IterateUntil(
                    x => x.NextCommand(rollFunc, console),
                    x => x.IsGameComplete())
            .Iter(x => console.WriteMessage(x.GetEndGameMessage())));

    private static GameState InitializeGame(IAnsiConsole console) =>
        console.ToIdentity()
               .Iter(x => DisplayTitle(x))
               .Iter(x => DisplayInstructions(x))
               .Map(x => StateMachine.InitialState());

    private static void DisplayTitle(IAnsiConsole console) =>
        new FigletText(Constants.GameTitle)
            .Centered()
            .Color(Color.Green)
            .ToIdentity()
            .Iter(x => console.Write(x))
            .Iter(x => console.WriteLine());

    private static void DisplayInstructions(IAnsiConsole console) =>
        console.Confirm(Constants.ShowInstructionsLabel)
               .ToIdentity()
               .Iter(x => console.WriteMessageConditional(x, Constants.Instructions))
               .Iter(x => x.IfTrueOrElse(() => console.PromptAnyKey(Constants.StartGameLabel)));

    private static GameState NextCommand(this GameState state, Func<int> rnd, IAnsiConsole console) =>
        GatherBetCommand.Handle(console, state.Zchips)
            .ToIdentity()
            .Map(bet => state with { CurrentBet = bet })
            .Map(s => StateMachine.NextState(s, rnd)
            .Iter(s => s.DisplayRoundEnd(console)));

    private static void DisplayRoundEnd(this GameState state, IAnsiConsole console) =>
        state.Iter(s => DicePresenter.RenderRow(console, s.Result.Rolls))
             .Iter(s => console.WriteMessage(Constants.TextLeftOffset, s.LatestMove))
             .Iter(s => console.ContinueOnAnyKey(Constants.NextRoundLabel, Constants.TextLeftOffset));

    private static string[] GetEndGameMessage(this GameState endState) =>
        endState.IsGameLoss() ? Constants.LostGameMsg : Constants.WonGameMsg;
}
