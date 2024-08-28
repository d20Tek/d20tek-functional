using D20Tek.Minimal.Functional;
using Games.Common;

namespace Zorgos;

internal static class StateMachine
{
    public static GameState InitialState() =>
        new(Constants.StartingChips, 0, [], RollResult.Empty);

    public static GameState NextState(GameState state, Func<int> rnd) =>
        Enumerable.Range(1, 2).Select(x => rnd()).ToArray()
            .Map(rolls => CalculatePayout(rolls, state.CurrentBet))
            .Map(result => (state.Zchips + result.ChipDelta).OrZero()
                .Map(total => state with
                {
                    Zchips = total,
                    CurrentBet = 0,
                    Result = result,
                    LatestMove = [string.Empty, result.Output, Constants.RollResult.Total(total)]
                }));

    private static RollResult CalculatePayout(int[] rolls, int bet) =>
        rolls[0] == rolls[1]
            ? new (rolls, 2 * bet, Constants.RollResult.Double)
            : rolls.Sum() switch
              {
                  6 or 7 => new (rolls, 0, Constants.RollResult.Push),
                  10 or 11 => new (rolls, 3 * bet, Constants.RollResult.Triple),
                  _ => new (rolls, -bet, Constants.RollResult.Lost)
              };
}
