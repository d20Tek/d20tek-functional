using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;

namespace SlotMachine;

internal static class Game
{
    public static void Play(IAnsiConsole console, Func<int> rollFunc) =>
    InitializeGame()
        .Map(initialState =>
            initialState.IterateUntil(
                x => x.NextState(rollFunc, console),
                x => GameCalculations.IsGameComplete(x))
        .Apply(x => console.WriteMessage(x.GetEndGameMessage())));


    private static GameState InitializeGame() => new(Constants.StartingTokens);

    private static GameState NextState(this GameState state, Func<int> rnd, IAnsiConsole console) =>
        state.Apply(s => DisplayRoundStart(s.Tokens, console))
             .Map(s => GameCalculations.GetRandomCombination(rnd, Constants.Fruit)
                 .Apply(combo => DisplayRow(console, combo))
                 .Map(combo => GameCalculations.CalculatePull(s.Tokens, combo))
                 .Apply(y => DisplayRoundEnd(console, y))
                 .Map(result =>  s with
                 {
                     Tokens = result.Tokens,
                     Round = s.Round + 1
                 }));

    private static void DisplayRoundStart(int tokens, IAnsiConsole console) =>
        console.Apply(c => c.Clear())
               .Apply(c => c.Write(Presenters.GameHeader(Constants.GameTitle)))
               .Apply(c => c.WriteMessage(Constants.CurrentTokensMessage(tokens)))
               .Apply(c => c.PromptAnyKey(Constants.PullLeverMessage));

    private static void DisplayRow(IAnsiConsole console, string[] items) =>
        console.WriteMessage(Constants.FruitRow(items));

    private static void DisplayRoundEnd(IAnsiConsole console, PullResult result) =>
        console.Apply(c => c.WriteMessage(result.Message))
               .Apply(c => c.PromptAnyKey(Constants.ContinueMessage));
}
