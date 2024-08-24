namespace MartianTrail.GamePhases;

internal static class GameCalculations
{

    public static int CalculateChargesUsed(decimal accuracy) =>
        (int)(50 * (1 - accuracy));

    public static int CalculateFoodGained(decimal accuracy) =>
        (int)(100 * accuracy);

    public static bool IsHuntingArea(Func<int> rnd, bool isWilderness) =>
        rnd() > (isWilderness ? 10 : 20);

    public static bool IsHuntingAvailable(Func<int> rnd) =>
        rnd() > 33;

    public static bool IsTradingPost(Func<int> rnd, bool isWilderness) =>
        rnd() > (isWilderness ? 90 : 10);

    public static bool IsWilderness(Func<int> rnd) =>
        rnd() > 33;
}