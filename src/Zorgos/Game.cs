using D20Tek.Minimal.Functional;
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
            .Apply(x => console.WriteMessage(x.GetEndGameMessage())));

    private static GameState InitializeGame(IAnsiConsole console) =>
        console.Apply(x => DisplayTitle(x))
               .Apply(x => DisplayInstructions(x))
               .Map(x => StateMachine.InitialState());

    private static void DisplayTitle(IAnsiConsole console) =>
        new FigletText(Constants.GameTitle)
            .Centered()
            .Color(Color.Green)
            .Apply(x => console.Write(x))
            .Apply(x => console.WriteLine());

    private static void DisplayInstructions(IAnsiConsole console) =>
        console.Confirm(Constants.ShowInstructionsLabel)
            .Apply(x => console.WriteMessageConditional(x, Constants.Instructions))
            .Apply(x => x.IfTrueOrElse(() => console.PromptAnyKey(Constants.StartGameLabel)));

    private static GameState NextCommand(this GameState state, Func<int> rnd, IAnsiConsole console) =>
        GatherBetCommand.Handle(console, state.Zchips)
            .Map(bet => state with { CurrentBet = bet })
            .Map(s => StateMachine.NextState(s, rnd)
            .Apply(s => s.DisplayRoundEnd(console)));

    private static void DisplayRoundEnd(this GameState state, IAnsiConsole console) =>
        state.Apply(s => DicePresenter.RenderRow(console, s.Result.Rolls))
             .Apply(s => console.WriteMessage(Constants.TextLeftOffset, s.LatestMove))
             .Apply(s => console.ContinueOnAnyKey(Constants.NextRoundLabel, Constants.TextLeftOffset));

    private static string[] GetEndGameMessage(this GameState endState) =>
        endState.IsGameLoss() ? Constants.LostGameMsg : Constants.WonGameMsg;
}
