using D20Tek.Functional;
using Spectre.Console;

namespace SlotMachine;

internal static class GameCalculations
{
    private const int _jackpot = 20;
    private const int _triple = 5;
    private const int _double = 2;
    private const int _loss = -1;

    public static string[] GetRandomCombination(Func<int> rnd, string[] fruit) =>
        Enumerable.Range(1, Constants.NumColumns)
                  .Select(_ => rnd() - 1)
                  .ToIdentity()
                  .Map(rolls => rolls.Select(x => fruit[x]).ToArray());

    public static PullResult CalculatePull(int tokens, string[] combo) =>
        CalculatePull(tokens, combo[Constants.a], combo[Constants.b], combo[Constants.c]);

    public static PullResult CalculatePull(int tokens, string a, string b, string c) =>
        (a == b && b == c)
            ? a == Constants.Cherry ? new PullResult(tokens + _jackpot, Constants.JackpotMessage)
                                    : new PullResult(tokens + _triple, Constants.TripleMessage)
            : (a == b || a == c || b == c)
                ? new PullResult(tokens + _double, Constants.DoubleMessage)
                : new PullResult(tokens + _loss, Constants.LossMessage);

    public static bool IsGameComplete(this GameState state) =>
        (state.Tokens <= 0 || state.Tokens >= Constants.EndingTokens);

    public static string[] GetEndGameMessage(this GameState endState) =>
        (endState.Tokens <= 0)
            ? Constants.GameLostMessage(endState.Round)
            : Constants.GameWonMessage(endState.Round);
}